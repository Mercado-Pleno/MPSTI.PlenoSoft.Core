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
		Task<(HttpStatusCode HttpStatus, string Content)> Notificar(Message message);
		Task<(HttpStatusCode HttpStatus, string Content)> IniciarProcesso(ProcessInstance processInstance);
		Task<IList<ExternalTask>> BuscarExternalTasks();
		Task<(HttpStatusCode HttpStatus, string Content)> CompletarExternalTask(ExternalTask task, Variables variables = null);
		Task<(HttpStatusCode HttpStatus, string Content)> ReportarErroExternalTask(ExternalTask task, Exception exception, Variables variables = null);
		Task<(HttpStatusCode HttpStatus, string Content)> ReportarFailureExternalTask(ExternalTask task, Exception exception, Variables variables = null);
		Task<bool> HealthCheck();
	}
}