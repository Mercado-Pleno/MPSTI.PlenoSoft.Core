using System;
using System.Collections;
using System.Collections.Generic;

namespace MPSC.PlenoSoft.Core.Collections.Generic
{
	public class Lista<T> : IEnumerable<T>
	{
		private readonly List<T> _lista = new List<T>();
		private readonly Action<T> _onAdicionar;
		private Func<IEnumerable<T>> _filler;
		public Int32 Count { get { return _lista.Count; } }

		public Lista(Action<T> onAdicionar)
		{
			_onAdicionar = onAdicionar;
		}

		public void Preencher(Func<IEnumerable<T>> filler)
		{
			_filler = filler;
		}

		public Lista<T> Adicionar(IEnumerable<T> lista)
		{
			foreach (var item in lista)
				Adicionar(item);
			return this;
		}

		public T Adicionar(T item)
		{
			_onAdicionar(item);
			_lista.Add(item);
			return item;
		}

		public void Remover(IEnumerable<T> items)
		{
			foreach (var item in items)
				_lista.Remove(item);
		}

		public void Remover(T item)
		{
			_lista.Remove(item);
		}

		public void Limpar()
		{
			_lista.Clear();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		private IEnumerator<T> GetEnumeratorImplementation()
		{
			var filler = _filler;
			_filler = null;
			if (filler != null)
				Adicionar(filler());
			return _lista.GetEnumerator();
		}

		public override String ToString()
		{
			return $"Lista<{typeof(T).Name}>(Count = {Count})";
		}
	}
}