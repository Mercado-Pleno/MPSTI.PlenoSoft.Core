using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MPSTI.PlenoSoft.Core.Utils.Abstracao;

namespace MPSTI.PlenoSoft.Core.Caches.Local
{
	public sealed class CacheOf<TEntidade> where TEntidade : IUniqueId
	{
		private readonly TimeSpan _timeOut;
		private readonly Func<IEnumerable<TEntidade>> _getCollectionFromSource;
		private readonly Action<TEntidade, long> _onAdd;
		private readonly List<TEntidade> _collection = new List<TEntidade>();
		private DateTime _lastUpdate = DateTime.Now;
		private long _id = 0;

		internal CacheOf(TimeSpan timeOut, Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, long> onAdd)
		{
			_timeOut = timeOut;
			_getCollectionFromSource = getCollectionFromSource;
			_onAdd = onAdd;
		}

		public bool EhValido => _collection.Any() && _lastUpdate.Add(_timeOut) > DateTime.Now;
		private List<TEntidade> ListaAtualizada => EhValido ? _collection : ColocarEmCache();

		public IEnumerable<TEntidade> Obter(Func<TEntidade, bool> filter)
		{
			return filter == null ? ListaAtualizada : ListaAtualizada.Where(filter);
		}

		public void Incluir(TEntidade entidade)
		{
			ListaAtualizada.Add(entidade);
			_onAdd?.Invoke(entidade, Interlocked.Increment(ref _id));
		}

		public void Alterar(TEntidade entidade)
		{
			Excluir(entidade);
			Incluir(entidade);
		}

		public void Excluir(TEntidade entidade)
		{
			ListaAtualizada.RemoveAll(d => d.UId == entidade.UId);
		}

		private List<TEntidade> ColocarEmCache()
		{
			var newCollection = _getCollectionFromSource?.Invoke();
			return AtualizarDados(newCollection ?? new TEntidade[0]);
		}

		private List<TEntidade> AtualizarDados(IEnumerable<TEntidade> newCollection)
		{
			_lastUpdate = DateTime.Now;
			_collection.Clear();
			_collection.AddRange(newCollection);
			_id = _collection.Any() ? Convert.ToInt64(_collection.Max(e => e.UId)) : 0L;
			return _collection;
		}
	}
}