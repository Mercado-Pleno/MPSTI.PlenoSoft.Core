using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Application
{
	public static class SwaggerConfiguration
	{
		private static IEnumerable<SwaggerDocument> GetSwaggerDocuments()
		{
			yield return new SwaggerDocument
			{
				Name = "v1",
				Title = "Azure Functions Http Trigger",
				Description = "Integrate Swagger UI With Azure Functions",
				Version = CoreAssembly<Startup>.FullVersion,
			};
		}

		private static void ConfigureDocOptionsAction(SwaggerDocOptions swaggerDocOptions)
		{
			swaggerDocOptions.AddCodeParameter = true;
			swaggerDocOptions.ConfigureSwaggerGen = ConfigureSwaggerGen;
			swaggerDocOptions.Documents = GetSwaggerDocuments();
			swaggerDocOptions.SpecVersion = OpenApiSpecVersion.OpenApi3_0;
		}

		private static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
		{
			swaggerGenOptions.CustomOperationIds(a => a.TryGetMethodInfo(out var value) ? value.Name : Guid.Empty.ToString());
		}

		public static IServiceCollection RegisterSwagger(this IServiceCollection serviceCollection, Assembly assembly = null)
			=> serviceCollection.AddSwashBuckle(assembly ?? Assembly.GetCallingAssembly(), ConfigureDocOptionsAction);


		[FunctionName("Swagger"), SwaggerIgnore]
		public static async Task<HttpResponseMessage> Swagger([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")] HttpRequestMessage req, [SwashBuckleClient] ISwashBuckleClient swasBuckleClient)
			=> await Task.FromResult(swasBuckleClient.CreateSwaggerJsonDocumentResponse(req));


		[FunctionName("SwaggerUI"), SwaggerIgnore]
		public static async Task<HttpResponseMessage> SwaggerUI([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")] HttpRequestMessage req, [SwashBuckleClient] ISwashBuckleClient swasBuckleClient)
			=> await Task.FromResult(swasBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
	}
}