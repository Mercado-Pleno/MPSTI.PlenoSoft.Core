using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public interface IRepository<TEntity> where TEntity : ICosmosDb
	{
		Task<TEntity> GetByPartitionKey(object partitionKey);

		Task<TEntity> Create(TEntity entity);

		Task<TEntity> CreateOrUpdate(TEntity entity);

		Task<TEntity> Delete(TEntity entity);
	}
}