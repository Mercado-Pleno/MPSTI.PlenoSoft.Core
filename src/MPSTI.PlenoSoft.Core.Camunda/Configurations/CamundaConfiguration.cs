using Microsoft.Extensions.DependencyInjection;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using MPSTI.PlenoSoft.Core.Camunda.Services;
using System;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Camunda.Configurations
{
    public static class CamundaConfiguration
    {
        public static IServiceCollection RegisterCamunda(this IServiceCollection services, string camundaBaseUrl, ExternalTaskConfig externalTaskConfig)
        {
            var camundaBaseAddress = camundaBaseUrl + (Path.EndsInDirectorySeparator(camundaBaseUrl) ? "" : "/");

            services.AddHttpClient("Camunda", httpClient => httpClient.BaseAddress = new Uri(camundaBaseAddress));
            services.AddSingleton(externalTaskConfig.ToExternalTaskFetchRequest());
            services.AddScoped<ICamundaClient, CamundaClient>();

            return services;
        }
    }
}