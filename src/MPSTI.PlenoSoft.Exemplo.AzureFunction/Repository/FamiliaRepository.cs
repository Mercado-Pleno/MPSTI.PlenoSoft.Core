using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public class FamiliaRepository : Repository<Familia>, IFamiliaRepository
	{
		public override string DatabaseName => "MercadoPleno";
		public override string ContainerName => "Familia";
		public FamiliaRepository(CosmosClient cosmosClient) : base(cosmosClient) { }
	}
}