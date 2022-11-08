using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Extensions
{
	public static class Retry
	{
		public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<T, bool> retryWhen, int retryCount = 3, Action<int> onRetry = null, int secondsSeed = 2)
		{
			return await CreateRetryPolicy(secondsSeed, retryCount, retryWhen, onRetry)
				.ExecuteAsync(action)
				.ConfigureAwait(false);
		}

		public static AsyncRetryPolicy<T> CreateRetryPolicy<T>(int secondsSeed, int retryCount, Func<T, bool> retryWhen, Action<int> onRetry)
		{
			return Policy
				.HandleResult(retryWhen)
				.WaitAndRetryAsync(
					retryCount: retryCount,
					sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(secondsSeed, attempt)),
					onRetry: (delegateResult, timeSpan, attempt, context) => onRetry?.Invoke(attempt)
				);
		}
	}
}