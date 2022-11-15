using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public abstract class Repository<TEntity>: IRepository<TEntity> where TEntity : ICosmosDb
	{
		private readonly CosmosClient _cosmosClient;
		private readonly string _databaseName;
		private readonly string _containerName;
		private readonly string _partitionKeyPath;
		private Database _database;
		private Container _container;

		public Repository(CosmosClient cosmosClient, string databaseName, string containerName, string partitionKeyPath)
		{
			_cosmosClient = cosmosClient;
			_databaseName = databaseName;
			_containerName = containerName;
			_partitionKeyPath = partitionKeyPath;
		}

		protected async Task<Database> GetDatabase() => _database ??=
			await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);

		protected async Task<Container> GetContainer() => _container ??=
			await (await GetDatabase()).CreateContainerIfNotExistsAsync(_containerName, _partitionKeyPath);

		public async Task<TEntity> GetByPartitionKey(object partitionKey)
		{
			return await Task.FromResult<TEntity>(default);
		}

		public async Task<TEntity> Create(TEntity entity)
		{
			var container = await GetContainer();
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var result = await container.CreateItemAsync(entity, partitionKey);
			return result.Resource;
		}

		public async Task<TEntity> CreateOrUpdate(TEntity entity)
		{
			var container = await GetContainer();
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var result = await container.UpsertItemAsync(entity, partitionKey);
			return result.Resource;
		}

		public async Task<TEntity> Delete(TEntity entity)
		{
			var container = await GetContainer();
			var partitionKey = new PartitionKey(entity.PartitionKeyValue);
			var result = await container.DeleteItemAsync<TEntity>(entity.Id, partitionKey);
			return result.Resource;
		}
	}
}
