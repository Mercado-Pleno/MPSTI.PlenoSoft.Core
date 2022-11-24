using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces
{
	public delegate void BatchAction<in TEntity>(string partitionKeyValue, IEnumerable<TEntity> items, TransactionalBatch transactionalBatch) where TEntity : ICosmosEntity;

	public delegate void BatchAction(string partitionKeyValue, TransactionalBatch transactionalBatch);

	public interface ICosmosRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity
	{
		Task<TCosmosEntity> InsertAsync(TCosmosEntity cosmosEntity);
		Task<TCosmosEntity> UpdateAsync(TCosmosEntity cosmosEntity);
		Task<TCosmosEntity> DeleteAsync(TCosmosEntity cosmosEntity);
		Task<TCosmosEntity> PatchAsync<TEntity>(TEntity entity, string path, string id, string partitionKeyValue);
		Task<ICosmosEntity> GetByPKAsIdAsync(string partitionKeyValue);
		Task<TCosmosEntity> GetAsync(TCosmosEntity cosmosEntity);
		Task<TCosmosEntity> GetAsync(string id, string partitionKeyValue);
		Task<TCosmosEntity> GetAsync(string id);
		Task<IEnumerable<TCosmosEntity>> GetAsync(IEnumerable<string> ids, string partitionKeyValue = null);
		Task<IEnumerable<TCosmosEntity>> GetAllAsync(string partitionKeyValue);
		Task<IEnumerable<TCosmosEntity>> GetAllAsync(QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> QueryAsync(string query, IDictionary<string, object> parameters = null, QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> QueryAsync(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions = null, string continuationToken = null);
		Task<TransactionalBatchResponse> ExecuteBatch(string partitionKeyValue, BatchAction batchAction);
		Task<Dictionary<string, TransactionalBatchResponse>> ExecuteBatch<TEntity>(IEnumerable<TEntity> lista, BatchAction<TEntity> batchAction) where TEntity : ICosmosEntity;
	}
}