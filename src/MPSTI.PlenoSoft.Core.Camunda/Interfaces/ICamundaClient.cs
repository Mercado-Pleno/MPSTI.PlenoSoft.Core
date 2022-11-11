using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Interfaces
{
	public interface ICamundaClient
	{
		Task<(HttpStatusCode HttpStatus, string Content)> SendMessage(Message message);
		Task<(HttpStatusCode HttpStatus, string Content)> StartProcess(ProcessInstance processInstance);
		Task<IList<ExternalTask>> FetchExternalTask();
		Task<(HttpStatusCode HttpStatus, string Content)> CompleteExternalTask(ExternalTask task, Variables variables = null);
		Task<(HttpStatusCode HttpStatus, string Content)> ReportBpmnErrorExternalTask(ExternalTask task, Exception exception, Variables variables = null);
		Task<(HttpStatusCode HttpStatus, string Content)> ReportFailureExternalTask(ExternalTask task, Exception exception, Variables variables = null);
		Task<bool> HealthCheck();
	}
}