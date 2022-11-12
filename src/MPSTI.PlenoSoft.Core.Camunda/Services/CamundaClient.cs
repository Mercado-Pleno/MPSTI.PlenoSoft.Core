using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Services
{
	public class CamundaClient : CamundaBaseClient, ICamundaClient
	{
		public CamundaClient(IHttpClientFactory httpClientFactory, ExternalTaskFetchRequest externalTaskFetchRequest)
			: base(httpClientFactory, externalTaskFetchRequest) { }

		public async Task<HttpResponseMessage> ProcessInstanceStart(ProcessInstance processInstance)
		{
			return await Protected($"process-definition/key/{processInstance.ProcessDefinitionName}/start", async resource =>
			{
				return await ExecuteAndValidate(resource, processInstance);
			});
		}

		public async Task<HttpResponseMessage> GlobalMessageSend(Message message)
		{
			return await Protected($"message", async resource =>
			{
				return await ExecuteAndValidate(resource, message);
			});
		}

		public async Task<IList<ExternalTask>> ExternalTaskFetchAndLock()
		{
			return await Protected($"external-task/fetchAndLock", async resource =>
			{
				var response = await Execute(resource, _fetchExternalTaskRequest);
				return await response.Content.ReadAsAsync<List<ExternalTask>>();
			});
		}

		public async Task<HttpResponseMessage> ExternalTaskComplete(ExternalTask task, Variables variables = null)
		{
			return await Protected($"external-task/{task.Id}/complete", async resource =>
			{
				var request = new ExternalTaskComplete(task, variables);
				var response = await Execute(resource, request);

				if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
					throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

				return await Validate(response);
			});
		}

		public async Task<HttpResponseMessage> ExternalTaskReportBpmnError(ExternalTask task, Exception exceptionError, Variables variables = null)
		{
			return await Protected($"external-task/{task.Id}/bpmnError", async resource =>
			{
				var request = new ExternalTaskError(task, exceptionError, variables);
				return await ExecuteAndValidate(resource, request);
			});
		}

		public async Task<HttpResponseMessage> ExternalTaskReportFailure(ExternalTask task, Exception exceptionError, Variables variables = null)
		{
			return await Protected($"external-task/{task.Id}/failure", async resource =>
			{
				var request = new ExternalTaskFailure(task, exceptionError, variables);
				return await ExecuteAndValidate(resource, request);
			});
		}
	}
}