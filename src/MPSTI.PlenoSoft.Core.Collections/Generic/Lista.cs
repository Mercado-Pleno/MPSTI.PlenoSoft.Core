using System;
using System.Collections;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Collections.Generic
{
	public class Lista<TItem> : IEnumerable<TItem>
	{
		private readonly List<TItem> _lista = new List<TItem>();
		private readonly Action<TItem> _onAdicionar;
		private readonly Action<TItem> _onRemover;
		private Func<IEnumerable<TItem>> _filler;
		public int Count => _lista.Count;

		public Lista(Action<TItem> onAdicionar, Action<TItem> onRemover = null)
		{
			_onAdicionar = onAdicionar;
			_onRemover = onRemover;
		}

		public void Preencher(Func<IEnumerable<TItem>> filler)
		{
			_filler = filler;
		}

		public Lista<TItem> Adicionar(IEnumerable<TItem> lista)
		{
			foreach (var item in lista)
				Adicionar(item);
			return this;
		}

		public TItem Adicionar(TItem item)
		{
			_lista.Add(item);
			_onAdicionar?.Invoke(item);
			return item;
		}

		public void Remover(IEnumerable<TItem> listaItem)
		{
			foreach (var item in listaItem)
				Remover(item);
		}

		public void Remover(TItem item)
		{
			_lista.Remove(item);
			_onRemover?.Invoke(item);
		}

		public void Limpar()
		{
			foreach (var item in _lista)
				_onRemover?.Invoke(item);
			_lista.Clear();
		}

		IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		private IEnumerator<TItem> GetEnumeratorImplementation()
		{
			if (_filler != null)
			{
				lock (this)
				{
					if (_filler != null)
					{
						Adicionar(_filler.Invoke());
						_filler = null;
					}
				}
			}
			return _lista.GetEnumerator();
		}

		public override string ToString()
		{
			return $"Lista<{typeof(TItem).Name}>(Count = {Count})";
		}

		public static IEnumerable<TItem> Empty { get { yield break; } }
	}
}