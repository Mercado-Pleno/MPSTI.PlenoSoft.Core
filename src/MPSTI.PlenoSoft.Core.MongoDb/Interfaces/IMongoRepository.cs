using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Interfaces
{
	public interface IMongoRepository<TMongoEntity> where TMongoEntity : IMongoEntity
	{
		Task<TMongoEntity> InsertAsync(TMongoEntity entity);
		Task<ReplaceOneResult> UpdateAsync(TMongoEntity entity);
		Task<DeleteResult> DeleteAsync(TMongoEntity entity);
		Task<DeleteResult> DeleteAsync(string id);
		Task<TMongoEntity> GetAsync(TMongoEntity entity);
		Task<TMongoEntity> GetAsync(string id);
		Task<bool> ExistsAsync(string id);
		Task<IEnumerable<TMongoEntity>> GetAll();
		Task<IAsyncCursor<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter);
	}
}