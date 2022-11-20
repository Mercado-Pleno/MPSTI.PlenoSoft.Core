using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public abstract class CosmosRepository<TCosmosEntity> : ICosmosRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity, new()
	{
		protected const string DefaultQuery = "Select * From C ";
		protected readonly Container Container;

		public virtual string PartitionKeyPath => new TCosmosEntity().PartitionKeyPath;
		public abstract string DatabaseName { get; }
		public abstract string ContainerName { get; }

		public CosmosRepository(CosmosClient cosmosClient) => Container = cosmosClient.GetContainer(DatabaseName, ContainerName);

		protected async Task<Container> CreateDatabaseAndContainer(CosmosClient cosmosClient)
		{
			var database = (await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName)).Database;
			return (await database.CreateContainerIfNotExistsAsync(ContainerName, PartitionKeyPath)).Container;
		}

		public async Task<TCosmosEntity> CreateItem(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var result = await Container.CreateItemAsync<TCosmosEntity>(entity, partitionKey);
			return result.Resource;
		}

		public async Task<TCosmosEntity> UpdateItem(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(entity);
			var result = await Container.UpsertItemAsync(entity, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> DeleteItem(TCosmosEntity entity)
		{
			if (entity == null) return default;
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var itemRequestOptions = new ItemRequestOptions().WithETag(entity);
			var result = await Container.DeleteItemAsync<TCosmosEntity>(entity.Id, partitionKey, itemRequestOptions);
			return result.Resource;
		}

		public async Task<TCosmosEntity> GetByItem(TCosmosEntity entity)
			=> (entity == null) ? default : await GetByIdAndPK(entity.Id, entity.PartitionKeyValue);

		public async Task<TCosmosEntity> GetByIdAndPK(string id, string partitionKeyValue)
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

		public async Task<TCosmosEntity> GetByIdOnly(string id)
		{
			var parameters = new Dictionary<string, object> { { "@id", id } };
			var results = await Query($"{DefaultQuery} Where C.id = @id", parameters);
			return results.SingleOrDefault();
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAllByPK(string partitionKeyValue)
		{
			var queryRequestOptions = new QueryRequestOptions().WithPartitionKey(partitionKeyValue);
			return await Query(new QueryDefinition(DefaultQuery), queryRequestOptions);
		}

		public async Task<IEnumerable<TCosmosEntity>> GetAll(QueryRequestOptions queryRequestOptions = null)
			=> await Query(new QueryDefinition(DefaultQuery), queryRequestOptions);

		public async Task<IEnumerable<TCosmosEntity>> Query(string query, IDictionary<string, object> parameters = null, QueryRequestOptions queryRequestOptions = null)
		{
			var queryDefinition = new QueryDefinition(query ?? DefaultQuery).WithParameters(parameters);
			return await Query(queryDefinition, queryRequestOptions);
		}

		public async Task<IEnumerable<TCosmosEntity>> Query(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions = null, string continuationToken = null)
		{
			var requestOptions = (queryRequestOptions ?? new QueryRequestOptions()).WithMaxItemCount();
			using var feedIterator = Container.GetItemQueryIterator<TCosmosEntity>(queryDefinition, continuationToken, requestOptions);
			return await feedIterator.GetResult();
		}

		public async Task<TransactionalBatchResponse> ExecuteBatch(string partitionKeyValue, Action<TransactionalBatch> batchAction)
		{
			var transactionalBatch = Container.CreateTransactionalBatch(new PartitionKey(partitionKeyValue));

			batchAction?.Invoke(transactionalBatch);

			return await transactionalBatch.ExecuteAsync();
		}
	}
}