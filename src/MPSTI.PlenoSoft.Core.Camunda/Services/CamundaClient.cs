using MPSTI.PlenoSoft.Core.Camunda.Configurations;
using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages;
using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Services
{
	public class CamundaClient : ICamundaClient
	{
		private readonly HttpClient _httpClient;
		private readonly ExternalTaskFetchRequest _fetchExternalTaskRequest;

		public CamundaClient(IHttpClientFactory httpClientFactory, ExternalTaskConfig externalTaskConfig)
		{
			_httpClient = httpClientFactory.CreateClient("Camunda");
			_fetchExternalTaskRequest = new ExternalTaskFetchRequest
			{
				MaxTasks = externalTaskConfig.MaxTasks,
				WorkerId = externalTaskConfig.WorkerId,
				Topics = externalTaskConfig.Topics,
			};
		}

		public async Task<(HttpStatusCode HttpStatus, string Content)> SendMessage(Message message)
		{
			try
			{
				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"message", message),
					retryWhen: response => !response.IsSuccessStatusCode
				);

				return await Validate(response);
			}
			catch (Exception exception)
			{
				throw new CamundaException($"Houve um problema ao {nameof(SendMessage)} no {nameof(CamundaClient)} com a url: [{_httpClient.BaseAddress}]", exception);
			}
		}

		public async Task<(HttpStatusCode HttpStatus, string Content)> StartProcess(ProcessInstance processInstance)
		{
			try
			{
				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"process-definition/key/{processInstance.ProcessDefinitionName}/start", processInstance),
					retryWhen: response => !response.IsSuccessStatusCode
				);

				return await Validate(response);
			}
			catch (Exception exception)
			{
				throw new CamundaException($"Houve um problema ao IniciarProcesso no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", exception);
			}
		}

		public async Task<IList<ExternalTask>> FetchExternalTask()
		{
			try
			{
				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"external-task/fetchAndLock", _fetchExternalTaskRequest),
					retryWhen: response => !response.IsSuccessStatusCode,
					retryCount: 1
				);

				return await response.Content.ReadAsAsync<IList<ExternalTask>>();
			}
			catch (Exception exception)
			{
				throw new CamundaException($"Houve um problema ao BuscarExternalTasks no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", exception);
			}
		}

		public async Task<(HttpStatusCode HttpStatus, string Content)> CompleteExternalTask(ExternalTask task, Variables variables = null)
		{
			try
			{
				var request = new ExternalTaskComplete(task, variables);

				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"external-task/{task.Id}/complete", request),
					retryWhen: response => !response.IsSuccessStatusCode
				);

				if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
					throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

				return await Validate(response);
			}
			catch (Exception exception)
			{
				throw new CamundaException($"Houve um problema ao CompletarExternalTask no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", exception);
			}
		}

		public async Task<(HttpStatusCode HttpStatus, string Content)> ReportBpmnErrorExternalTask(ExternalTask task, Exception exception, Variables variables = null)
		{
			try
			{
				var request = new ExternalTaskError(task, exception, variables);

				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"external-task/{task.Id}/bpmnError", request),
					retryWhen: response => !response.IsSuccessStatusCode
				);

				return await Validate(response);
			}
			catch (Exception ex)
			{
				throw new CamundaException($"Houve um problema ao ReportarErroExternalTask no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", ex);
			}
		}

		public async Task<(HttpStatusCode HttpStatus, string Content)> ReportFailureExternalTask(ExternalTask task, Exception exception, Variables variables = null)
		{
			try
			{
				var request = new ExternalTaskFailure(task, exception, variables);

				var response = await Retry.ExecuteAsync(
					action: () => _httpClient.PostAsJsonAsync($"external-task/{task.Id}/failure", request),
					retryWhen: response => !response.IsSuccessStatusCode
				);

				return await Validate(response);
			}
			catch (Exception ex)
			{
				throw new CamundaException($"Houve um problema ao ReportarErroExternalTask no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", ex);
			}
		}

		public async Task<bool> HealthCheck()
		{
			try
			{
				var response = await _httpClient.GetAsync($"engine");
				return response.IsSuccessStatusCode;
			}
			catch (Exception exception)
			{
				throw new CamundaException($"Houve um problema ao fazer HealthCheck no Proxy do Camunda com a url: [{_httpClient.BaseAddress}]", exception);
			}
		}

		private static async Task<(HttpStatusCode HttpStatus, string Content)> Validate(HttpResponseMessage response)
		{
			var responseString = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException(responseString, null, response.StatusCode);
			return (response.StatusCode, responseString);
		}
	}
}