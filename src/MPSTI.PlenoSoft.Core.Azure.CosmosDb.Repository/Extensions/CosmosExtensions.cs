using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Extensions
{
	[DebuggerNonUserCode]
	public static class CosmosExtensions
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
}