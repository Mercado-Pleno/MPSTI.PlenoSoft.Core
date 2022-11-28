using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Contracts
{
	public abstract class CosmosEntityConcurrent<TId> : CosmosEntity<TId>, ICosmosEntityConcurrent
	{
		#region // "ICosmosEntityConcurrent"
		[JsonProperty("_etag")]
		public virtual string ETag { get; set; }
		#endregion // "ICosmosEntityConcurrent"
	}
}