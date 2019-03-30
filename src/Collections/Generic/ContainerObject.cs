using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPSC.PlenoSoft.Core.Collections.Generic
{
	public interface IContainerBuilder
	{
		IContainerBuilder Adicionar<TItem>(TItem item, params TItem[] items);
		IContainerBuilder Adicionar<TItem>(IEnumerable<TItem> items);
		ContainerObject Build();
	}

	public class ContainerObject : IEnumerable<Object>, IContainerBuilder
	{
		private readonly Dictionary<Type, IList> _containerLists = new Dictionary<Type, IList>();
		public virtual IEnumerable<Object> Objects => _containerLists.Values.SelectMany(v => v.OfType<Object>());
		public virtual IEnumerable<Object> SnapshotObjects => Objects.ToArray();

		public virtual IEnumerable<TItem> Obter<TItem>(Func<TItem, Boolean> filtro = null)
		{
			var lista = ObterLista<TItem>();
			return (filtro == null) ? lista : lista.Where(filtro);
		}

		public virtual TItem Adicionar<TItem>(TItem item)
		{
			var lista = ObterLista<TItem>();
			lista.Add(item);
			return item;
		}

		public virtual IEnumerable<TItem> Adicionar<TItem>(params TItem[] items)
		{
			return AdicionarItens(items);
		}

		public virtual IEnumerable<TItem> Adicionar<TItem>(IEnumerable<TItem> items)
		{
			return AdicionarItens(items);
		}

		public virtual TItem Remover<TItem>(TItem item)
		{
			var lista = ObterLista<TItem>();
			lista.Remove(item);
			return item;
		}

		public virtual void Remover<TItem>(Predicate<TItem> filtro)
		{
			var lista = ObterLista<TItem>() as List<TItem>;
			lista.RemoveAll(filtro);
		}

		private IEnumerable<TItem> AdicionarItens<TItem>(IEnumerable<TItem> items)
		{
			foreach (var item in items)
				Adicionar(item);
			return items;
		}

		private IList<TItem> ObterLista<TItem>()
		{
			var type = typeof(TItem);
			if (!_containerLists.TryGetValue(type, out var lista))
				lista = _containerLists[type] = new List<TItem>();
			return ((IList<TItem>)lista);
		}

		private IEnumerator<Object> GetEnumeratorImplementation()
		{
			return SnapshotObjects.GetEnumerator();
		}

		IEnumerator<Object> IEnumerable<Object>.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorImplementation();
		}

		IContainerBuilder IContainerBuilder.Adicionar<TItem>(TItem item, params TItem[] items)
		{
			Adicionar(item);
			AdicionarItens(items);
			return this;
		}

		IContainerBuilder IContainerBuilder.Adicionar<TItem>(IEnumerable<TItem> items)
		{
			AdicionarItens(items);
			return this;
		}

		ContainerObject IContainerBuilder.Build()
		{
			return this;
		}
	}
}