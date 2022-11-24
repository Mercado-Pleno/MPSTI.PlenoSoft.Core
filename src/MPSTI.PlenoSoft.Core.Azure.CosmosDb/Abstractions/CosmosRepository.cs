using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Abstractions
{
	public abstract class CosmosRepository<TCosmosEntity> : ICosmosRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity//, new()
	{
		public const string DefaultQuery = "Select * From C ";
		protected readonly Container Container;

		public abstract string DatabaseName { get; }
		public abstract string ContainerName { get; }
		public abstract string PartitionKeyPath { get; }

		protected CosmosRepository(CosmosClient cosmosClient)
		{
			if (cosmosClient is null) throw new ArgumentNullException(nameof(cosmosClient));
			Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
		}

		protected async Task<Container> CreateDatabaseAndContainer(CosmosClient cosmosClient)
		{
			var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName)).Database;
			return (await database.CreateContainerIfNotExistsAsync(ContainerName, PartitionKeyPath)).Container;
		}

		public async Task<TCosmosEntity> InsertAsync(TCosmosEntity cosmosEntity)
		{
			if (cosmosEntity == null) return default;
			var partitionKey = new PartitionKey(cosmosEntity.PartitionKeyValue);
			var result = await Container.CreateItemAsync(cosmosEntity, partitionKey);
			return result.Resource;
		}

		public async Task<TCosmosEntity> UpdateAsync(TCosmosEntity cosmosEntity)
		{
			if (cosmosEntity == null) return default;
			var partitionKey = new PartitionKey(cosmosEntity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(cosmosEntity);
			var result = await Container.UpsertItemAsync(cosmosEntity, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> DeleteAsync(TCosmosEntity cosmosEntity)
		{
			if (cosmosEntity == null) return default;
			var partitionKey = new PartitionKey(cosmosEntity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(cosmosEntity);
			var result = await Container.DeleteItemAsync<TCosmosEntity>(cosmosEntity.Id, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> PatchAsync<TEntity>(TEntity entity, string path, string id, string partitionKeyValue)
		{
			if ((entity == null) || string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(partitionKeyValue)) return default;
			var partitionKey = new PartitionKey(partitionKeyValue);
			var patchOperations = new[] { PatchOperation.Replace(path, entity) };
			var patchItemRequestOptions = new PatchItemRequestOptions().WithETag(entity);
			var result = await Container.PatchItemAsync<TCosmosEntity>(id, partitionKey, patchOperations, patchItemRequestOptions);
			return result.Resource;
		}

		public async Task<ICosmosEntity> GetByPKAsIdAsync(string partitionKeyValue)
			=> string.IsNullOrWhiteSpace(partitionKeyValue) ? default : await GetAsync(partitionKeyValue, partitionKeyValue);

		public async Task<TCosmosEntity> GetAsync(TCosmosEntity cosmosEntity)
			=> cosmosEntity == null ? default : await GetAsync(cosmosEntity.Id, cosmosEntity.PartitionKeyValue);

		public async Task<TCosmosEntity> GetAsync(string id, string partitionKeyValue)
		{
			if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(partitionKeyValue)) return default;
			try
			{
				var partitionKey = new PartitionKey(partitionKeyValue);
				var result = await Container.ReadItemAsync<TCosmosEntity>(id, partitionKey);
				return result.Resource;
			}
			catch (CosmosException cosmosException) when (cosmosException.StatusCode == HttpStatusCode.NotFound)
			{
				return default;
			}
		}

		public async Task<TCosmosEntity> GetAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return default;
			var parameters = new Dictionary<string, object> { { "@id", id } };
			var results = await QueryAsync($"{DefaultQuery} Where C.id = @id", parameters);
			return results.SingleOrDefault();
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAsync(IEnumerable<string> ids, string partitionKeyValue = null)
		{
			if (ids?.Any() != true) return Array.Empty<TCosmosEntity>();
			var parameters = new Dictionary<string, object> { { "@ids", ids } };
			var queryRequestOptions = new QueryRequestOptions().WithPartitionKey(partitionKeyValue);
			return await QueryAsync($"{DefaultQuery} Where ARRAY_CONTAINS(@ids, C.id)", parameters, queryRequestOptions);
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAllAsync(string partitionKeyValue)
		{
			if (string.IsNullOrWhiteSpace(partitionKeyValue)) return Array.Empty<TCosmosEntity>();
			var queryRequestOptions = new QueryRequestOptions().WithPartitionKey(partitionKeyValue);
			return await QueryAsync(new QueryDefinition(DefaultQuery), queryRequestOptions);
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAllAsync(QueryRequestOptions queryRequestOptions = null)
			=> await QueryAsync(new QueryDefinition(DefaultQuery), queryRequestOptions);

		public async Task<IEnumerable<TCosmosEntity>> QueryAsync(string query, IDictionary<string, object> parameters = null, QueryRequestOptions queryRequestOptions = null)
		{
			var queryDefinition = new QueryDefinition(query ?? DefaultQuery).WithParameters(parameters);
			return await QueryAsync(queryDefinition, queryRequestOptions);
		}

		public async Task<IEnumerable<TCosmosEntity>> QueryAsync(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions = null, string continuationToken = null)
		{
			var requestOptions = (queryRequestOptions ?? new QueryRequestOptions()).WithMaxItemCount();
			using var feedIterator = Container.GetItemQueryIterator<TCosmosEntity>(queryDefinition, continuationToken, requestOptions);
			return await feedIterator.GetResult();
		}

		public async Task<TransactionalBatchResponse> ExecuteBatch(string partitionKeyValue, BatchAction batchAction)
		{
			var transactionalBatch = Container.CreateTransactionalBatch(new PartitionKey(partitionKeyValue));

			batchAction?.Invoke(partitionKeyValue, transactionalBatch);

			return await transactionalBatch.ExecuteAsync();
		}

		public async Task<Dictionary<string, TransactionalBatchResponse>> ExecuteBatch<TEntity>(IEnumerable<TEntity> lista, BatchAction<TEntity> batchAction) where TEntity : ICosmosEntity
		{
			var results = new Dictionary<string, TransactionalBatchResponse>();
			var groups = lista.GroupBy(g => g.PartitionKeyValue);

			foreach (var group in groups)
				results[group.Key] = await ExecuteBatch(group.Key, (partitionKeyValue, action) => batchAction(partitionKeyValue, group, action));

			return results;
		}
	}
}