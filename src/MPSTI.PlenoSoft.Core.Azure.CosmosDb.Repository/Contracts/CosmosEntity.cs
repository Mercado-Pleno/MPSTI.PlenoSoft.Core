using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Contracts
{
	public abstract class CosmosEntity<TId> : ICosmosEntity
	{
		#region // "ICosmosEntity"
		string ICosmosEntity.Id => Id?.ToString();
		string ICosmosEntity.PartitionKeyValue => PartitionKeyValue;
		#endregion // "ICosmosEntity"

		[JsonProperty("id")]
		public virtual TId Id { get; set; }

		[JsonIgnore]
		protected abstract string PartitionKeyValue { get; }
	}
}