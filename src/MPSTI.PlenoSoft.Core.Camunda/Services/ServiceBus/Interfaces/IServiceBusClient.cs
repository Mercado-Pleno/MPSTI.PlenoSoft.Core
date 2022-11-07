using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces
{
	public interface IServiceBusClient
	{
		Task EnviarMensagem(string serviceBusConnectionString, EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null);

		Task EnviarMensagemParaFila(string serviceBusConnectionString, string entityPath, Message message);

		Task EnviarMensagemParaTopico(string serviceBusConnectionString, string entityPath, Message message);
	}
}