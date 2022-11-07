using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Exceptions
{
	public static class ExceptionExtensions
	{
		public static string Messages(this AggregateException exception, string joinSeparator = "\r\n")
		{
			return string.Join(joinSeparator, exception.GetAllMessages());
		}

		public static IEnumerable<string> GetAllMessages(this AggregateException exception)
		{
			return exception?.InnerExceptions?.SelectMany(e => e.GetMessages()) ?? Enumerable.Empty<string>();
		}

		public static IEnumerable<string> GetMessages(this Exception exception)
		{
			while (exception != null)
			{
				yield return exception.Message;
				exception = exception.InnerException;
			}
		}

		public static string Messages(this Exception exception, string joinSeparator = "\r\n")
		{
			return string.Join(joinSeparator, exception.GetMessages());
		}
	}
}