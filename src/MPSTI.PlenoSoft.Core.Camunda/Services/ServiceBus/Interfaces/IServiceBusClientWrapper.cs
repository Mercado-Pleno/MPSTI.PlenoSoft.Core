using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces
{
	public interface IServiceBusClientWrapper
	{
		Task CreateAsync(EntityType entityType, string entityPath, params string[] subscribers);

		Task CreateQueueAsync(string entityPath);

		Task CreateTopicAsync(string entityPath, params string[] subscribers);

		Task EnviarMensagem(EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null);

		Task EnviarMensagemParaFila(string entityPath, Message message);

		Task EnviarMensagemParaTopico(string entityPath, Message message);
	}
}