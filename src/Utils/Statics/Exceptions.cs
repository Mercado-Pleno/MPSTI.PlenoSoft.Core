using System;
using System.Collections.Generic;

namespace MPSC.PlenoSoft.Core.Utils.Statics
{
	public static class Exceptions
	{
		public static String Messages(this Exception exception, String join = " -> ")
		{
			return String.Join(join, exception.Message());
		}

		public static IEnumerable<String> Message(this Exception exception)
		{
			while (exception != null)
			{
				yield return exception.Message;
				exception = exception.InnerException;
			}
		}
	}
}