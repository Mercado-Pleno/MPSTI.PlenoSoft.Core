using System;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts
{
    public class Aluno : ICosmosDb
    {
        [JsonIgnore]
        string ICosmosDb.Id => Id.ToString();

		[JsonIgnore]
		string ICosmosDb.PartitionKeyValue => cpf;

        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string cpf { get; set; }
    }
}
