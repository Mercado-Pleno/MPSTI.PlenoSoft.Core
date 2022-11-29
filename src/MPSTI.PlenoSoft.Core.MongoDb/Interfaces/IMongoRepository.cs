using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Interfaces
{
	public interface IMongoRepository<TMongoEntity, TId> where TMongoEntity : IMongoEntity<TId>
	{
		Task<TMongoEntity> InsertAsync(TMongoEntity entity);
		Task<ReplaceOneResult> UpdateAsync(TMongoEntity entity);
		Task<DeleteResult> DeleteAsync(TMongoEntity entity);
		Task<DeleteResult> DeleteAsync(TId id);
		Task<TMongoEntity> GetAsync(TMongoEntity entity);
		Task<TMongoEntity> GetAsync(TId id);
		Task<bool> ExistsAsync(TId id);
		Task<IEnumerable<TMongoEntity>> GetAll();
		Task<IAsyncCursor<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter);
	}
}