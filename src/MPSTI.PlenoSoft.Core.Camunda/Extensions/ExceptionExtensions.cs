using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Camunda.Extensions
{
	public static class ExceptionExtensions
	{
		public static string Messages(this Exception exception, string join = "\r\n") => string.Join(join, exception.GetMessages());

		public static IEnumerable<string> GetMessages(this Exception exception)
		{
			while (exception != null)
			{
				yield return exception.Message;
				exception = exception.InnerException;
			}
		}
	}
}