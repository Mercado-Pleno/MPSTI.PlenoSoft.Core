using System;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts
{
    public class Familia : ICosmosEntity, ICosmosConcurrency
	{
        [JsonIgnore]
        string ICosmosId.Id => Id.ToString();

		[JsonIgnore]
		string ICosmosPK.PartitionKeyPath => $"/{nameof(LastName)}";

		[JsonIgnore]
		string ICosmosPK.PartitionKeyValue => LastName;

        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cpf { get; set; }
        public DateTime Date { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; set; }

        public Familia()
        {
            Date = DateTime.UtcNow;
        }
    }
}
