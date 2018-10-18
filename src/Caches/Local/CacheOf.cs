using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MPSC.PlenoSoft.Core.Utils.Abstracao;

namespace MPSC.PlenoSoft.Core.Caches.Local
{
	public sealed class CacheOf<TEntidade> where TEntidade : IUniqueId
	{
		private readonly TimeSpan _timeOut;
		private readonly Func<IEnumerable<TEntidade>> _getCollectionFromSource;
		private readonly Action<TEntidade, Int64> _onAdd;
		private readonly List<TEntidade> _collection = new List<TEntidade>();
		private DateTime _lastUpdate = DateTime.Now;
		private Int64 _id = 0;

		internal CacheOf(TimeSpan timeOut, Func<IEnumerable<TEntidade>> getCollectionFromSource, Action<TEntidade, Int64> onAdd)
		{
			_timeOut = timeOut;
			_getCollectionFromSource = getCollectionFromSource;
			_onAdd = onAdd;
		}

		public Boolean EhValido => (_collection.Any() && (_lastUpdate.Add(_timeOut) > DateTime.Now));
		private List<TEntidade> ListaAtualizada => EhValido ? _collection : ColocarEmCache();

		public IEnumerable<TEntidade> Obter(Func<TEntidade, Boolean> filter)
		{
			return (filter == null) ? ListaAtualizada : ListaAtualizada.Where(filter);
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