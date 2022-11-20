using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
	public interface IFamiliaRepository : ICosmosRepository<Familia> { }
}