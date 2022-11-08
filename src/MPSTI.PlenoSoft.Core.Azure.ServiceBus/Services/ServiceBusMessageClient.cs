using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs.ServiceBus;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Services
{
    public class ServiceBusMessageClient : IServiceBusMessageClient
    {
        private readonly string _serviceBusConnectionString;

        public ServiceBusMessageClient(string serviceBusConnectionString) => _serviceBusConnectionString = serviceBusConnectionString;

        public async Task EnviarMensagem(EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null)
        {
            var message = CreateMessage(mensagem, agendarPara);
            if (entityType == EntityType.Queue)
                await EnviarMensagemParaFila(entityPath, message);
            else
                await EnviarMensagemParaTopico(entityPath, message);
        }

        public async Task EnviarMensagemParaFila(string entityPath, Message message)
        {
            var senderClient = new QueueClient(_serviceBusConnectionString, entityPath);
            await SendAsync(senderClient, message);
        }

        public async Task EnviarMensagemParaTopico(string entityPath, Message message)
        {
            var senderClient = new TopicClient(_serviceBusConnectionString, entityPath);
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