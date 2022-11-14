using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Configurations;
using MPSTI.PlenoSoft.Core.Camunda.Configurations;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Application;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Application
{
	public class Startup : FunctionsStartup
	{
		private readonly IConfigurationRoot _configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

		public Startup() => SBConfig.Setup(_configuration);

		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.Configure<IConfiguration>(_configuration);

			Configure(builder.Services, _configuration);
		}

		public static void Configure(IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient();

			services.RegisterServiceBus(configuration.GetValue<string>("Azure_ServiceBus"));
			services.RegisterCamunda(configuration.GetValue<string>("Camunda_Url"), new());
		}
	}
}