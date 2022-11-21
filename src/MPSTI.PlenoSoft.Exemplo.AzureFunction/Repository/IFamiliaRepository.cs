using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public interface IFamiliaRepository : ICosmosRepository<Familia> { }
}