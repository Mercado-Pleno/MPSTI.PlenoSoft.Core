using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Contracts
{
	public abstract class CosmosEntityConcurrent<TId> : CosmosEntity<TId>, ICosmosEntityConcurrent
	{
		#region // "ICosmosEntityConcurrent"
		[JsonProperty("_etag")]
		public virtual string ETag { get; set; }
		#endregion // "ICosmosEntityConcurrent"
	}
}