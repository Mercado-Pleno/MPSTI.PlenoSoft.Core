using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces
{
	public delegate void BatchAction<TEntity>(string partitionKeyValue, IEnumerable<TEntity> items, TransactionalBatch transactionalBatch) where TEntity : ICosmosEntity;

	public delegate void BatchAction(string partitionKeyValue, TransactionalBatch transactionalBatch);

	public interface ICosmosRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity
	{
		Task<TCosmosEntity> CreateItem(TCosmosEntity entity);
		Task<TCosmosEntity> UpdateItem(TCosmosEntity entity);
		Task<TCosmosEntity> DeleteItem(TCosmosEntity entity);
		Task<TCosmosEntity> GetByItem(TCosmosEntity entity);
		Task<TCosmosEntity> GetByIdAndPK(string id, string partitionKeyValue = null);
		Task<TCosmosEntity> GetByIdOnly(string id);
		Task<IEnumerable<TCosmosEntity>> GetAllByPK(string partitionKeyValue);
		Task<IEnumerable<TCosmosEntity>> GetAll(QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> Query(string query, IDictionary<string, object> parameters = null, QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> Query(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions = null, string continuationToken = null);
		Task<TransactionalBatchResponse> ExecuteBatch(string partitionKeyValue, BatchAction batchAction);
		Task<Dictionary<string, TransactionalBatchResponse>> ExecuteBatch<TEntity>(IEnumerable<TEntity> lista, BatchAction<TEntity> batchAction) where TEntity : ICosmosEntity;
	}
}