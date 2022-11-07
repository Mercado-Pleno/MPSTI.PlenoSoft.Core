using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs.ServiceBus;
using MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus
{
	public class ServiceBusClient : IServiceBusClient
	{
		public async Task EnviarMensagem(string serviceBusConnectionString, EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null)
		{
			var message = CreateMessage(mensagem, agendarPara);
			if (entityType == EntityType.Queue)
				await EnviarMensagemParaFila(serviceBusConnectionString, entityPath, message);
			else
				await EnviarMensagemParaTopico(serviceBusConnectionString, entityPath, message);
		}

		public async Task EnviarMensagemParaFila(string serviceBusConnectionString, string entityPath, Message message)
		{
			var senderClient = new QueueClient(serviceBusConnectionString, entityPath);
			await SendAsync(senderClient, message);
		}

		public async Task EnviarMensagemParaTopico(string serviceBusConnectionString, string entityPath, Message message)
		{
			var senderClient = new TopicClient(serviceBusConnectionString, entityPath);
			await SendAsync(senderClient, message);
		}

		private static async Task SendAsync(ISenderClient senderClient, Message message)
		{
			await senderClient.SendAsync(message);
			await senderClient.CloseAsync();
		}

		private static Message CreateMessage(string mensagem, TimeSpan? agendarPara)
		{
			var messageArray = Encoding.UTF8.GetBytes(mensagem);
			var message = new Message(messageArray);

			if (agendarPara.HasValue)
				message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(agendarPara.Value);

			return message;
		}
	}
}