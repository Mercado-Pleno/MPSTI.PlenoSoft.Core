using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSC.PlenoSoft.ControlFlux.Utils
{
	public static class CollectionExtension
	{
		public static Boolean In<T>(this T self, params T[] args)
		{
			return args.Contains(self);
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
				action?.Invoke(item);
		}
	}
}