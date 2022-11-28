using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Interfaces;
using System.Diagnostics;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Extensions
{
	[DebuggerNonUserCode]
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