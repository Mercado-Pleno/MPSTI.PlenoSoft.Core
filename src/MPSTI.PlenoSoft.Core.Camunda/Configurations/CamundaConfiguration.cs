using Microsoft.Extensions.DependencyInjection;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using MPSTI.PlenoSoft.Core.Camunda.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


namespace MPSTI.PlenoSoft.Core.Camunda.Configurations
{
	public static class CamundaConfiguration
	{
		public static JsonSerializerSettings JsonSerializerSettings { get; set; } = GetDefaultJsonSerializerSettings();

		public static IServiceCollection RegisterCamunda(this IServiceCollection services, string camundaBaseUrl, ExternalTaskConfig externalTaskConfig)
		{
			var camundaBaseAddress = camundaBaseUrl + (Path.EndsInDirectorySeparator(camundaBaseUrl) ? "" : "/");

			services.AddHttpClient("Camunda", httpClient => httpClient.BaseAddress = new Uri(camundaBaseAddress));
			services.AddSingleton(externalTaskConfig.ToExternalTaskFetchRequest());
			services.AddScoped<ICamundaClient, CamundaClient>();

			return services;
		}

		private static JsonSerializerSettings GetDefaultJsonSerializerSettings()
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter> { new StringEnumConverter() },
				Culture = new CultureInfo("en-US"),
				DefaultValueHandling = DefaultValueHandling.Ignore,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				Formatting = Formatting.None,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			};

			return jsonSerializerSettings;
		}
	}
}