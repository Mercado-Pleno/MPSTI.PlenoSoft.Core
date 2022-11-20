using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public interface IRepository<TCosmosEntity> where TCosmosEntity : ICosmosEntity
	{
		Task<TCosmosEntity> CreateItem(TCosmosEntity entity);
		Task<TCosmosEntity> UpdateItem(TCosmosEntity entity);
		Task<TCosmosEntity> DeleteItem(TCosmosEntity entity);
		Task<TCosmosEntity> GetByItem(TCosmosEntity entity);
		Task<TCosmosEntity> GetByIdWithPK(string id, string partitionKeyValue = null);
		Task<TCosmosEntity> GetByIdOnly(string id);
		Task<TCosmosEntity> GetByPKOnly(string partitionKeyValue);
		Task<IEnumerable<TCosmosEntity>> GetAllByPK(string partitionKeyValue);
		Task<IEnumerable<TCosmosEntity>> GetAll(QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> Query(string query, IDictionary<string, object> parameters = null, QueryRequestOptions queryRequestOptions = null);
		Task<IEnumerable<TCosmosEntity>> Query(QueryDefinition queryDefinition, QueryRequestOptions queryRequestOptions = null, string continuationToken = null);
		Task<TransactionalBatchResponse> ExecuteBatch(string partitionKeyValue, Action<TransactionalBatch> batchAction);
	}
}