using MPSTI.PlenoSoft.Core.MongoDb.Interfaces;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.MongoDb.Contracts
{
	public abstract class MongoEntity : IMongoEntity
	{
		[JsonProperty("id")]
		public virtual string Id { get; set; }
	}
}