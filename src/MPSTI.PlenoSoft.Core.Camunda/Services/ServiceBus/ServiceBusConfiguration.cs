using Microsoft.Extensions.DependencyInjection;
using MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus
{
	public static class ServiceBusConfiguration
	{
		public static IServiceCollection RegisterServiceBus(this IServiceCollection services, string serviceBusConnectionString)
		{
			services.AddSingleton<IServiceBusClient, ServiceBusClient>();
			services.AddSingleton<IServiceBusManager, ServiceBusManager>();
			services.AddSingleton<IServiceBusClientWrapper, ServiceBusClientWrapper>(sp =>
			{
				var serviceBusClient = sp.GetRequiredService<IServiceBusClient>();
				var serviceBusManager = sp.GetRequiredService<IServiceBusManager>();
				return new ServiceBusClientWrapper(serviceBusConnectionString, serviceBusClient, serviceBusManager);
			});

			return services;
		}
	}
}