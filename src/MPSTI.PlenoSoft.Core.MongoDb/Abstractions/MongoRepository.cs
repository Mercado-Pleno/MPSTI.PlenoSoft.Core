using MongoDB.Driver;
using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Abstractions
{
	public abstract class MongoRepository<TMongoEntity> : IMongoRepository<TMongoEntity> where TMongoEntity : IMongoEntity
	{
		protected readonly IMongoCollection<TMongoEntity> Collection;

		public abstract string DatabaseName { get; }
		public abstract string CollectionName { get; }

		public MongoRepository(IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(DatabaseName);
			Collection = database.GetCollection<TMongoEntity>(CollectionName);
		}

		public async Task<TMongoEntity> CreateItem(TMongoEntity entity)
		{
			if (entity == null) return default;
			await Collection.InsertOneAsync(entity);
			return entity;
		}

		public async Task<ReplaceOneResult> UpdateItem(TMongoEntity entity)
		{
			if (entity == null) return default;
			return await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
		}

		public async Task<DeleteResult> DeleteItem(TMongoEntity entity)
			=> entity == null ? default : await DeleteItem(entity.Id);

		public async Task<DeleteResult> DeleteItem(string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return default;
			return await Collection.DeleteOneAsync(e => e.Id == id);
		}

		public async Task<TMongoEntity> GetByItem(TMongoEntity entity)
			=> entity == null ? default : await GetById(entity.Id);

		public async Task<TMongoEntity> GetById(string id)
		{
			if (string.IsNullOrWhiteSpace(id)) return default;
			var entities = await Collection.FindAsync(e => e.Id == id);
			return entities.SingleOrDefault();
		}

		public async Task<IEnumerable<TMongoEntity>> GetAll() => await Query(e => true);

		public async Task<IEnumerable<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter)
		{
			var cursor = await Collection.FindAsync(filter);
			return await cursor.ToListAsync();
		}
	}
}