using MPSTI.PlenoSoft.Camunda.Contratos;
using MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks;
using MPSTI.PlenoSoft.Camunda.Contratos.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Interfaces
{
	public interface IProxyCamunda
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