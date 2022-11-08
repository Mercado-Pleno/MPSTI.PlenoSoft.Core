using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Services
{
	public class ServiceBusWrapperClient : IServiceBusWrapperClient
	{
		private readonly IServiceBusMessageClient _serviceBusMessageClient;
		private readonly IServiceBusManagerClient _serviceBusManagerClient;

		public ServiceBusWrapperClient(IServiceBusMessageClient serviceBusMessageClient, IServiceBusManagerClient serviceBusManagerClient)
		{
			_serviceBusMessageClient = serviceBusMessageClient;
			_serviceBusManagerClient = serviceBusManagerClient;
		}

		public async Task CreateAsync(EntityType entityType, string entityPath, params string[] subscribers)
			=> await _serviceBusManagerClient.CreateAsync(entityType, entityPath, subscribers);

		public async Task CreateQueueAsync(string entityPath)
			=> await _serviceBusManagerClient.CreateQueueAsync(entityPath);

		public async Task CreateTopicAsync(string entityPath, params string[] subscribers)
			=> await _serviceBusManagerClient.CreateTopicAsync(entityPath, subscribers);

		public async Task EnviarMensagem(EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null)
			=> await _serviceBusMessageClient.EnviarMensagem(entityType, entityPath, mensagem, agendarPara);

		public async Task EnviarMensagemParaFila(string entityPath, Message message)
			=> await _serviceBusMessageClient.EnviarMensagemParaFila(entityPath, message);

		public async Task EnviarMensagemParaTopico(string entityPath, Message message)
			=> await _serviceBusMessageClient.EnviarMensagemParaTopico(entityPath, message);
	}
}