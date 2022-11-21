using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;

namespace MPSTI.PlenoSoft.Core.MongoDb.Configurations
{
	public static class MongoConfiguration
	{
		public static IServiceCollection RegisterMongoDb(this IServiceCollection services, string connectionStringMongoDb)
		{
			services.AddSingleton(sp => new MongoClient(connectionStringMongoDb));
			services.AddSingleton<IMongoClient>(sp => sp.GetRequiredService<MongoClient>());
			return services;
		}
	}
}