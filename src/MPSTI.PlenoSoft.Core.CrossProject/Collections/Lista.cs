using System;
using System.Collections;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.CrossProject.Collections
{
	public class Lista<TItem> : IEnumerable<TItem>
	{
		private readonly object _access = new object();
		private readonly List<TItem> _lista = new List<TItem>();
		private readonly Action<TItem> _onAdd;
		private readonly Action<TItem> _onRemove;
		private Func<IEnumerable<TItem>> _onFill;
		public int Count => _lista.Count;

		public Lista(Action<TItem> onAdd, Action<TItem> onRemove = null)
		{
			_onAdd = onAdd;
			_onRemove = onRemove;
		}

		public void SetFill(Func<IEnumerable<TItem>> onFill) => _onFill = onFill;

		public Lista<TItem> Add(IEnumerable<TItem> lista)
		{
			if (lista != null)
			{
				foreach (var item in lista)
					Add(item);
			}
			return this;
		}

		public TItem Add(TItem item)
		{
			_onAdd?.Invoke(item);
			_lista.Add(item);
			return item;
		}

		public void Remove(IEnumerable<TItem> items)
		{
			foreach (var item in items)
				Remove(item);
		}

		public void Remove(TItem item)
		{
			_onRemove?.Invoke(item);
			_lista.Remove(item);
		}

		public void Limpar()
		{
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
			if (_onFill != null)
			{
				lock (_access)
				{
					Add(_onFill?.Invoke());
					_onFill = null;
				}
			}

			return _lista.GetEnumerator();
		}

		public override string ToString()
		{
			return $"Lista<{typeof(TItem).Name}>(Count = {Count})";
		}
	}
}