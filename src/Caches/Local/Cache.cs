using System;
using System.Collections.Generic;
using MPSC.PlenoSoft.Core.Utils.Abstracao;

namespace MPSC.PlenoSoft.Core.Caches.Local
{
	public partial class Cache
	{
		private readonly Dictionary<Type, Object> _caches = new Dictionary<Type, Object>();
		private readonly TimeSpan _timeOut;
		public Cache(TimeSpan timeOut) => _timeOut = timeOut;

		public IEnumerable<TEntidade> Setup<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd = null) where TEntidade : IUniqueId
		{
			var cache = ObterCache(getCollectionFromSource, onAdd);
			return cache.Obter(null);
		}

		public IEnumerable<TEntidade> Obter<TEntidade>(Func<TEntidade, Boolean> filter) where TEntidade : IUniqueId
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

		private CacheOf<TEntidade> ObterCache<TEntidade>(Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd) where TEntidade : IUniqueId
		{
			var type = typeof(TEntidade);
			if (!_caches.TryGetValue(type, out var cache))
				cache = _caches[type] = new CacheOf<TEntidade>(_timeOut, getCollectionFromSource, onAdd);
			return ((CacheOf<TEntidade>)cache);
		}

		private IEnumerable<TEntidade> Empty<TEntidade>() { yield break; }
	}
}