using System;
using System.Collections.Generic;
using MPSTI.PlenoSoft.Core.Utils.Abstracao;

namespace MPSTI.PlenoSoft.Core.Caches.Local
{
	public partial class Cache
	{
		private readonly Dictionary<Type, object> _caches = new Dictionary<Type, object>();
		private readonly TimeSpan _timeOut;
		public Cache(TimeSpan timeOut) => _timeOut = timeOut;

		public IEnumerable<TEntidade> Setup<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, long> onAdd = null) where TEntidade : IUniqueId
		{
			var cache = ObterCache(getCollectionFromSource, onAdd);
			return cache.Obter(null);
		}

		public IEnumerable<TEntidade> Obter<TEntidade>(Func<TEntidade, bool> filter) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			return cache.Obter(filter);
		}

		public void Incluir<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Incluir(entidade);
		}

		public void Excluir<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Excluir(entidade);
		}

		public void Alterar<TEntidade>(TEntidade entidade) where TEntidade : IUniqueId
		{
			var cache = ObterCache(Empty<TEntidade>, null);
			cache.Alterar(entidade);
		}

		private CacheOf<TEntidade> ObterCache<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, long> onAdd) where TEntidade : IUniqueId
		{
			var type = typeof(TEntidade);
			if (!_caches.TryGetValue(type, out var cache))
				cache = _caches[type] = new CacheOf<TEntidade>(_timeOut, getCollectionFromSource, onAdd);
			return (CacheOf<TEntidade>)cache;
		}

		private IEnumerable<TEntidade> Empty<TEntidade>() { yield break; }
	}
}