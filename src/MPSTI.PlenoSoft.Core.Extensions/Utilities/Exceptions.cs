using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
	public static class Exceptions
	{
		public static string Messages(this Exception exception, string join = " - ")
		{
			return string.Join(join, exception.Message());
		}

		public static IEnumerable<string> Message(this Exception exception)
		{
			while (exception != null)
			{
				yield return exception.Message;
				exception = exception.InnerException;
			}
		}
	}
}