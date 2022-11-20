using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public interface ICosmosEntityConcurrent
	{
		string ETag { get; }
	}

	public abstract class CosmosEntityConcurrent<TId> : CosmosEntity<TId>, ICosmosEntityConcurrent
	{
		#region // "ICosmosConcurrency"
		[JsonProperty("_etag")]
		public virtual string ETag { get; set; }
		#endregion // "ICosmosConcurrency"
	}
}