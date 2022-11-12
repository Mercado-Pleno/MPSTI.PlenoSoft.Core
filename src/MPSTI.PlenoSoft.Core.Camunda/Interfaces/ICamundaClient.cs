using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Interfaces
{
	public interface ICamundaClient
	{
		Task<bool> HealthCheck();
		Task<HttpResponseMessage> GlobalMessageSend(Message message);
		Task<HttpResponseMessage> ProcessInstanceStart(ProcessInstance processInstance);
		Task<IList<ExternalTask>> ExternalTaskFetchAndLock();
		Task<HttpResponseMessage> ExternalTaskComplete(ExternalTask task, Variables variables = null);
		Task<HttpResponseMessage> ExternalTaskReportBpmnError(ExternalTask task, Exception exception, Variables variables = null);
		Task<HttpResponseMessage> ExternalTaskReportFailure(ExternalTask task, Exception exception, Variables variables = null);
	}
}