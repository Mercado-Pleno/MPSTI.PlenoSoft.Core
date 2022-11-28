using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Abstractions;
using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Interfaces;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public interface IFamiliaRepository : ICosmosRepository<Familia> { }

	public class FamiliaRepository : CosmosRepository<Familia>, IFamiliaRepository
	{
		public override string PartitionKeyPath => "/LastName";
		public override string DatabaseName => "MercadoPleno";
		public override string ContainerName => "Familia";

		public FamiliaRepository(CosmosClient cosmosClient) : base(cosmosClient)
		{
			CreateDatabaseAndContainer(cosmosClient).GetAwaiter().GetResult();
		}
	}
}