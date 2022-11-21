using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Extensions
{
	public static class CosmosItemRequestOptionsExtensions
	{
		public static TItemRequestOptions WithETag<TItemRequestOptions, TCosmosEntity>(this TItemRequestOptions itemRequestOptions, TCosmosEntity entity) where TItemRequestOptions : ItemRequestOptions
			=> itemRequestOptions.WithETag(entity as ICosmosEntityConcurrent);

		public static TItemRequestOptions WithETag<TItemRequestOptions>(this TItemRequestOptions itemRequestOptions, ICosmosEntityConcurrent entity) where TItemRequestOptions : ItemRequestOptions
		{
			itemRequestOptions.IfMatchEtag = entity?.ETag;
			return itemRequestOptions;
		}
	}
}