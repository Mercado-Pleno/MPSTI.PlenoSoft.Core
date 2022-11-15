using Microsoft.Azure.Cosmos;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository
{
    public class AlunoRepository : Repository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(CosmosClient cosmosClient) : base(cosmosClient, "MercadoPleno", "Aluno", "/cpf") { }
    }
}
