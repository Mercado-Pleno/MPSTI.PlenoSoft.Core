using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.MongoDb.Contracts
{
	public abstract class MongoEntity<TId> : IMongoEntity
	{
		#region // "IMongoEntity"
		string IMongoEntity.Id => Id?.ToString();
		#endregion // "IMongoEntity"

		[JsonProperty("id")]
		public virtual TId Id { get; set; }
	}
}