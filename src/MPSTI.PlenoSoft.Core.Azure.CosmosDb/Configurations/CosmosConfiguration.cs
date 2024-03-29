﻿using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Configurations
{
	[ExcludeFromCodeCoverage]
	public static class CosmosConfiguration
	{
		public static IServiceCollection RegisterCosmosDb(this IServiceCollection services, string connectionStringCosmosDb)
		{
			var cosmosClientOptions = new CosmosClientOptions { ConnectionMode = ConnectionMode.Direct };
			services.AddSingleton(sp => new CosmosClient(connectionStringCosmosDb, cosmosClientOptions));
			return services;
		}

		public static IServiceCollection RegisterCosmosDb(this IServiceCollection services, string accountEndpoint, string authKeyOrResourceToken)
		{
			var cosmosClientOptions = new CosmosClientOptions { ConnectionMode = ConnectionMode.Direct };
			services.AddSingleton(sp => new CosmosClient(accountEndpoint, authKeyOrResourceToken, cosmosClientOptions));
			return services;
		}
	}
}