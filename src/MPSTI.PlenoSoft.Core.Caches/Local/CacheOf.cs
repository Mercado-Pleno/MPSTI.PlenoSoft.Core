using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MPSTI.PlenoSoft.Core.Extensions.Abstracts;

namespace MPSTI.PlenoSoft.Core.Caches.Local
{
	public sealed class CacheOf<TEntity> where TEntity : IEntity
	{
		private readonly TimeSpan _timeOut;
		private readonly Func<IEnumerable<TEntity>> _getCollectionFromSource;
		private readonly Action<TEntity, long> _onAdd;
		private readonly List<TEntity> _collection = new List<TEntity>();
		private DateTime _lastUpdate = DateTime.Now;
		private long _id = 0;

		internal CacheOf(TimeSpan timeOut, Func<IEnumerable<TEntity>> getCollectionFromSource, Action<TEntity, long> onAdd)
		{
			_timeOut = timeOut;
			_getCollectionFromSource = getCollectionFromSource;
			_onAdd = onAdd;
		}

		public bool EhValido => _collection.Any() && _lastUpdate.Add(_timeOut) > DateTime.Now;
		private List<TEntity> ListaAtualizada => EhValido ? _collection : ColocarEmCache();

		public IEnumerable<TEntity> Obter(Func<TEntity, bool> filter)
		{
			return filter == null ? ListaAtualizada : ListaAtualizada.Where(filter);
		}

		public void Incluir(TEntity entidade)
		{
			ListaAtualizada.Add(entidade);
			_onAdd?.Invoke(entidade, Interlocked.Increment(ref _id));
		}

		public void Alterar(TEntity entidade)
		{
			Excluir(entidade);
			Incluir(entidade);
		}

		public void Excluir(TEntity entidade)
		{
			ListaAtualizada.RemoveAll(d => d.Id?.Equals(entidade.Id) == true);
		}

		private List<TEntity> ColocarEmCache()
		{
			var newCollection = _getCollectionFromSource?.Invoke();
			return AtualizarDados(newCollection ?? new TEntity[0]);
		}

		private List<TEntity> AtualizarDados(IEnumerable<TEntity> newCollection)
		{
			_lastUpdate = DateTime.Now;
			_collection.Clear();
			_collection.AddRange(newCollection);
			_id = _collection.Any() ? Convert.ToInt64(_collection.Max(e => e.Id)) : 0L;
			return _collection;
		}
	}
}