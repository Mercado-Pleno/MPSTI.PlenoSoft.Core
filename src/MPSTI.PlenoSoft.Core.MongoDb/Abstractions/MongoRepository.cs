using MongoDB.Driver;
using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Abstractions
{
	public abstract class MongoRepository<TMongoEntity, TId> : IMongoRepository<TMongoEntity, TId> where TMongoEntity : IMongoEntity<TId>
		where TId : class
	{
		protected readonly IMongoCollection<TMongoEntity> Collection;

		public abstract string DatabaseName { get; }
		public abstract string CollectionName { get; }

		protected MongoRepository(IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(DatabaseName);
			Collection = database.GetCollection<TMongoEntity>(CollectionName);
		}

		public async Task<TMongoEntity> InsertAsync(TMongoEntity entity)
		{
			if (entity == null) return default;
			await Collection.InsertOneAsync(entity);
			return entity;
		}

		public async Task<ReplaceOneResult> UpdateAsync(TMongoEntity entity)
		{
			if (entity == null) return default;
			return await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
		}

		public async Task<DeleteResult> DeleteAsync(TMongoEntity entity)
			=> entity == null ? default : await DeleteAsync(entity.Id);

		public async Task<DeleteResult> DeleteAsync(TId id)
		{
			if (IsDefault(id)) return default;
			return await Collection.DeleteOneAsync(e => e.Id == id);
		}

		public async Task<TMongoEntity> GetAsync(TMongoEntity entity)
			=> entity == null ? default : await GetAsync(entity.Id);

		public async Task<TMongoEntity> GetAsync(TId id)
		{
			if (IsDefault(id)) return default;
			var entities = await Query(e => e.Id == id);
			return await entities.SingleOrDefaultAsync();
		}

		public async Task<bool> ExistsAsync(TId id) => !IsDefault(id) && await (await Query(e => e.Id == id)).AnyAsync();

		public async Task<IEnumerable<TMongoEntity>> GetAll() => await (await Query(e => true)).ToListAsync();

		public async Task<IAsyncCursor<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter)
			=> await Collection.FindAsync(filter);

		private bool IsDefault(TId id)
		{
			return (id == default(TId)) 
				|| ReferenceEquals(id as object, null)
				|| ((id is string stringValue) && string.IsNullOrWhiteSpace(stringValue))
			;
		}
	}
}