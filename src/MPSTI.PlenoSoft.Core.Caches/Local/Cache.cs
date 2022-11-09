using System;
using System.Collections.Generic;
using System.Linq;
using MPSTI.PlenoSoft.Core.Extensions.Abstracts;

namespace MPSTI.PlenoSoft.Core.Caches.Local
{
	public partial class Cache
	{
		private readonly Dictionary<Type, object> _caches = new Dictionary<Type, object>();
		private readonly TimeSpan _timeOut;
		public Cache(TimeSpan timeOut) => _timeOut = timeOut;

		public IEnumerable<TEntity> Setup<TEntity>(Func<IEnumerable<TEntity>> getCollectionFromSource, Action<TEntity, long> onAdd = null) where TEntity : IEntity
		{
			var cache = ObterCache(getCollectionFromSource, onAdd);
			return cache.Obter(null);
		}

		public IEnumerable<TEntity> Obter<TEntity>(Func<TEntity, bool> filter) where TEntity : IEntity
		{
			var cache = ObterCache(Enumerable.Empty<TEntity>, null);
			return cache.Obter(filter);
		}

		public void Incluir<TEntity>(TEntity entidade) where TEntity : IEntity
		{
			var cache = ObterCache(Enumerable.Empty<TEntity>, null);
			cache.Incluir(entidade);
		}

		public void Excluir<TEntity>(TEntity entidade) where TEntity : IEntity
		{
			var cache = ObterCache(Enumerable.Empty<TEntity>, null);
			cache.Excluir(entidade);
		}

		public void Alterar<TEntity>(TEntity entidade) where TEntity : IEntity
		{
			var cache = ObterCache(Enumerable.Empty<TEntity>, null);
			cache.Alterar(entidade);
		}

		private CacheOf<TEntity> ObterCache<TEntity>(Func<IEnumerable<TEntity>> getCollectionFromSource, Action<TEntity, long> onAdd) where TEntity : IEntity
		{
			var type = typeof(TEntity);
			if (!_caches.TryGetValue(type, out var cache))
				cache = _caches[type] = new CacheOf<TEntity>(_timeOut, getCollectionFromSource, onAdd);
			return (CacheOf<TEntity>)cache;
		}
	}
}