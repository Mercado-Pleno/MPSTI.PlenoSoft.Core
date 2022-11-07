using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Collections
{
	public static class CollectionExtension
	{
		public static bool In<T>(this T self, params T[] listValues)
		{
			return listValues?.Contains(self) ?? false;
		}

		public static void ForEach<TItem>(this IEnumerable<TItem> collection, Action<TItem> action)
		{
			foreach (var item in collection)
				action?.Invoke(item);
		}
	}
}