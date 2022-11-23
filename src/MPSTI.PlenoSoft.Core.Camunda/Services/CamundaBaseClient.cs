using MPSTI.PlenoSoft.Core.Camunda.Configurations;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Services
{
	/// <summary>
	/// https://docs.camunda.io/docs/apis-clients/operate-api/
	/// </summary>
	public abstract class CamundaBaseClient
	{
		protected readonly HttpClient _httpClient;
		protected readonly ExternalTaskFetchRequest _fetchExternalTaskRequest;

		public CamundaBaseClient(IHttpClientFactory httpClientFactory, ExternalTaskFetchRequest externalTaskFetchRequest)
		{
			_httpClient = httpClientFactory.CreateClient("Camunda");
			_fetchExternalTaskRequest = externalTaskFetchRequest;
		}

		public async Task<bool> HealthCheck()
		{
			return await Protected("engine", async resource =>
			{
				var response = await _httpClient.GetAsync(resource);
				return response.IsSuccessStatusCode;
			});
		}

		protected async Task<TResponse> Protected<TResponse>(string resource, Func<string, Task<TResponse>> function)
		{
			try
			{
				return await function.Invoke(resource);
			}
			catch (Exception exception)
			{
				throw new CamundaException($"There was a problem calling '{resource}' in 'CamundaClient' with url: [{_httpClient.BaseAddress}{resource}]", exception);
			}
		}

		protected async Task<HttpResponseMessage> ExecuteAndValidate<TObject>(string resource, TObject request, int retryCount = 2)
		{
			var response = await Execute(resource, request, retryCount);
			return await Validate(response);
		}

		protected async Task<HttpResponseMessage> Execute<TBody>(string resource, TBody bodyContent, int retryCount = 2)
		{
			var content = new StringContent(JsonConvert.SerializeObject(bodyContent, CamundaConfiguration.JsonSerializerSettings), Encoding.UTF8, "application/json");
			return await Retry.ExecuteAsync(
				action: () => _httpClient.PostAsync(resource, content),
				retryWhen: response => !response.IsSuccessStatusCode,
				retryCount: retryCount
			);
		}

		protected async Task<HttpResponseMessage> Validate(HttpResponseMessage response)
		{
			var responseString = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException(responseString, null, response.StatusCode);
			return response;
		}
	}
}