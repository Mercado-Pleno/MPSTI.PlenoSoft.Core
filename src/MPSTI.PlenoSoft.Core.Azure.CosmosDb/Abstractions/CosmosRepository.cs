using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Abstractions
{
	public abstract class CosmosRepository<TCosmosEntity> : ICosmosRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity, new()
	{
		protected const string DefaultQuery = "Select * From C ";
		protected readonly Container Container;

		public virtual string PartitionKeyPath => new TCosmosEntity().PartitionKeyPath;
		public abstract string DatabaseName { get; }
		public abstract string ContainerName { get; }

		protected CosmosRepository(CosmosClient cosmosClient) => Container = cosmosClient.GetContainer(DatabaseName, ContainerName);

		protected async Task<Container> CreateDatabaseAndContainer(CosmosClient cosmosClient)
		{
			var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName)).Database;
			return (await database.CreateContainerIfNotExistsAsync(ContainerName, PartitionKeyPath)).Container;
		}

		public async Task<TCosmosEntity> InsertAsync(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var result = await Container.CreateItemAsync(entity, partitionKey);
			return result.Resource;
		}

		public async Task<TCosmosEntity> UpdateAsync(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(entity);
			var result = await Container.UpsertItemAsync(entity, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> DeleteAsync(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(entity);
			var result = await Container.DeleteItemAsync<TCosmosEntity>(entity.Id, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> PatchAsync<TEntity>(TEntity entity, string path, string id, string partitionKeyValue)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(partitionKeyValue);
			var patchOperations = new[] { PatchOperation.Replace(path, entity) };
			var patchItemRequestOptions = new PatchItemRequestOptions().WithETag(entity);
			var result = await Container.PatchItemAsync<TCosmosEntity>(id, partitionKey, patchOperations, patchItemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> GetAsync(TCosmosEntity entity)
			=> entity == null ? default : await GetAsync(entity.Id, entity.PartitionKeyValue);

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
			var parameters = new Dictionary<string, object> { { "@id", id } };
			var results = await QueryAsync($"{DefaultQuery} Where C.id = @id", parameters);
			return results.SingleOrDefault();
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAllAsync(string partitionKeyValue)
		{
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