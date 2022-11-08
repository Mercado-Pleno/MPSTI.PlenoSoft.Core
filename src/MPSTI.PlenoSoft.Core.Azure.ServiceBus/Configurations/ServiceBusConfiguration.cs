using Microsoft.Extensions.DependencyInjection;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Services;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Configurations
{
	public static class ServiceBusConfiguration
	{
		public static IServiceCollection RegisterServiceBus(this IServiceCollection services, string serviceBusAdministrativeConnectionString)
			=> services.RegisterServiceBus(serviceBusAdministrativeConnectionString, serviceBusAdministrativeConnectionString);

		public static IServiceCollection RegisterServiceBus(this IServiceCollection services, string serviceBusMessageConnectionString, string serviceBusManagerConnectionString)
		{
			services.AddSingleton<IServiceBusMessageClient, ServiceBusMessageClient>(sp => new ServiceBusMessageClient(serviceBusMessageConnectionString));
			services.AddSingleton<IServiceBusManagerClient, ServiceBusManagerClient>(sp => new ServiceBusManagerClient(serviceBusManagerConnectionString));
			services.AddSingleton<IServiceBusWrapperClient, ServiceBusWrapperClient>();

			return services;
		}
	}
}