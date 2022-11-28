using Microsoft.Azure.Cosmos;
using System.Diagnostics;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions
{
	[DebuggerNonUserCode]
	public static class CosmosQueryRequestOptionsExtensions
	{
		public static QueryRequestOptions WithMaxItemCount(this QueryRequestOptions queryRequestOptions, int? maxItemCount = 10, bool force = false)
		{
			if (force)
				queryRequestOptions.MaxItemCount = maxItemCount;
			else
				queryRequestOptions.MaxItemCount ??= maxItemCount;

			return queryRequestOptions;
		}

		public static QueryRequestOptions WithPartitionKey(this QueryRequestOptions queryRequestOptions, string partitionKeyValue = null)
		{
			if (!string.IsNullOrWhiteSpace(partitionKeyValue))
				queryRequestOptions.PartitionKey = new PartitionKey(partitionKeyValue);
			return queryRequestOptions;
		}
	}
}