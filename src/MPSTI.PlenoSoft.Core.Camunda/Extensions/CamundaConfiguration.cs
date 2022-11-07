using Microsoft.Extensions.DependencyInjection;
using MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks;
using MPSTI.PlenoSoft.Camunda.Interfaces;
using MPSTI.PlenoSoft.Camunda.Proxies;
using System;
using System.IO;

namespace MPSTI.PlenoSoft.Camunda.Extensions
{
	public static class CamundaConfiguration
	{
		public static IServiceCollection RegisterCamunda(this IServiceCollection services, ExternalTaskConfig externalTaskConfig, string camundaBaseUrl)
		{
			var camundaBaseAddress = camundaBaseUrl + (Path.EndsInDirectorySeparator(camundaBaseUrl) ? "" : "/");

			services.AddHttpClient("Camunda", httpClient => httpClient.BaseAddress = new Uri(camundaBaseAddress));
			services.AddSingleton(externalTaskConfig);
			services.AddScoped<IProxyCamunda, ProxyCamunda>();

			return services;
		}
	}
}