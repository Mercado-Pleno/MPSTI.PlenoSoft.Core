using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Interfaces
{
	public interface IMongoRepository<TMongoEntity> where TMongoEntity : IMongoEntity
	{
		Task<TMongoEntity> CreateItem(TMongoEntity entity);
		Task<ReplaceOneResult> UpdateItem(TMongoEntity entity);
		Task<DeleteResult> DeleteItem(TMongoEntity entity);
		Task<DeleteResult> DeleteItem(string id);
		Task<TMongoEntity> GetByItem(TMongoEntity entity);
		Task<TMongoEntity> GetById(string id);
		Task<IEnumerable<TMongoEntity>> GetAll();
		Task<IEnumerable<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter);
	}
}