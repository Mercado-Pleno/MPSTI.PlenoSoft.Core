using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public static class CosmosConfig
	{
		public static IServiceCollection RegisterCosmosDb(this IServiceCollection services, string connectionStringCosmosDb)
		{
			var cosmosClientOptions = new CosmosClientOptions { ConnectionMode = ConnectionMode.Direct };
			services.AddSingleton(sp => new CosmosClient(connectionStringCosmosDb, cosmosClientOptions));
			return services;
		}
	}
}