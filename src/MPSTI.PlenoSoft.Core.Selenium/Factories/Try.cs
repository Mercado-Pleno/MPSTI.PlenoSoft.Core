using System;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Selenium.Factories
{
	public static class Try
	{
		public static TResult Run<TResult>(int retry, Func<int, TResult> action, Func<int, TimeSpan> waitSeed = null)
		{
			var index = 0;
			while (index <= retry)
			{
				try
				{
					return action(index++);
				}
				catch (Exception)
				{
					var wait = waitSeed?.Invoke(index) ?? TimeSpan.FromMilliseconds(index);
					Thread.Sleep(wait);
				}
			}

			return default;
		}
	}
}