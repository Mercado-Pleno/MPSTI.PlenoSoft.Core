using MongoDB.Driver;
using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.MongoDb.Abstractions
{
	public abstract class MongoRepository<TMongoEntity, TId> : IMongoRepository<TMongoEntity, TId> where TMongoEntity : IMongoEntity<TId>
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
			return await Collection.ReplaceOneAsync(GetFilterById(entity.Id), entity);
		}

		public async Task<DeleteResult> DeleteAsync(TMongoEntity entity)
			=> entity == null ? default : await DeleteAsync(entity.Id);

		public async Task<DeleteResult> DeleteAsync(TId id)
		{
			if (IsDefaultValue(id)) return default;
			return await Collection.DeleteOneAsync(GetFilterById(id));
		}

		public async Task<TMongoEntity> GetAsync(TMongoEntity entity)
			=> entity == null ? default : await GetAsync(entity.Id);

		public async Task<TMongoEntity> GetAsync(TId id)
		{
			if (IsDefaultValue(id)) return default;
			var entities = await Query(GetFilterById(id));
			return await entities.SingleOrDefaultAsync();
		}

		public async Task<bool> ExistsAsync(TId id) => !IsDefaultValue(id) && await (await Query(GetFilterById(id))).AnyAsync();

		public async Task<IEnumerable<TMongoEntity>> GetAll() => await (await Query(e => true)).ToListAsync();

		public async Task<IAsyncCursor<TMongoEntity>> Query(Expression<Func<TMongoEntity, bool>> filter)
			=> await Collection.FindAsync(filter);

		protected abstract bool IsDefaultValue(TId id);

		/// <summary>
		/// Na maioria dos casos, você deve implementar esse método tão somente assim: "return e => e.Id == id;"
		/// implementação in-line deve ficar conforme abaixo:
		/// protected override Expression<Func<Order, bool>> GetFilterById(Guid id) => e => e.Id == id;
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract Expression<Func<TMongoEntity, bool>> GetFilterById(TId id);
	}
}