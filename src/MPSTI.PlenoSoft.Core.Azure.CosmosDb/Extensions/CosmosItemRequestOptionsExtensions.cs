using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions
{
	public static class CosmosItemRequestOptionsExtensions
	{
		public static ItemRequestOptions WithETag<TCosmosEntity>(this ItemRequestOptions itemRequestOptions, TCosmosEntity entity)
			=> itemRequestOptions.WithETag(entity as ICosmosEntityConcurrent);

		public static ItemRequestOptions WithETag(this ItemRequestOptions itemRequestOptions, ICosmosEntityConcurrent entity)
		{
			itemRequestOptions.IfMatchEtag = entity?.ETag;
			return itemRequestOptions;
		}
	}
}