using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public static class CosmosDbExtensions
	{
		public static QueryDefinition WithParameters(this QueryDefinition queryDefinition, IDictionary<string, object> parameters)
		{
			if (parameters != null)
			{
				foreach (var parameter in parameters)
					queryDefinition.WithParameter(parameter.Key, parameter.Value);
			}

			return queryDefinition;
		}

		public static async Task<IEnumerable<TEntity>> GetResult<TEntity>(this FeedIterator<TEntity> feedIterator)
		{
			var results = new List<TEntity>();
			while (feedIterator.HasMoreResults)
			{
				var response = await feedIterator.ReadNextAsync();
				results.AddRange(response.Resource);
			}
			return results;
		}
	}

	public static class CosmosDbItemRequestOptionsExtensions
	{
		public static ItemRequestOptions WithETag<TCosmosEntity>(this ItemRequestOptions itemRequestOptions, TCosmosEntity entity)
			=> WithETag(itemRequestOptions, entity as ICosmosConcurrency);

		public static ItemRequestOptions WithETag(this ItemRequestOptions itemRequestOptions, ICosmosConcurrency entity)
		{
			itemRequestOptions.IfMatchEtag = entity?.ETag;
			return itemRequestOptions;
		}
	}

	public static class CosmosDbQueryRequestOptionsExtensions
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