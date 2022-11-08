using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces
{
	public interface IServiceBusMessageClient
	{
		Task EnviarMensagem(EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null);

		Task EnviarMensagemParaFila(string entityPath, Message message);

		Task EnviarMensagemParaTopico(string entityPath, Message message);
	}
}