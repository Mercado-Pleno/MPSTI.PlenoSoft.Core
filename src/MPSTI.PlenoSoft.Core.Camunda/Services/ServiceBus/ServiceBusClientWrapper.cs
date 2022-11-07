using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;
using MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus
{
	public class ServiceBusClientWrapper : IServiceBusClientWrapper
	{
		private readonly string _serviceBusConnectionString;
		private readonly IServiceBusClient _serviceBusClient;
		private readonly IServiceBusManager _serviceBusManager;

		public ServiceBusClientWrapper(string serviceBusConnectionString, IServiceBusClient serviceBusClient, IServiceBusManager serviceBusManager)
		{
			_serviceBusConnectionString = serviceBusConnectionString;
			_serviceBusClient = serviceBusClient;
			_serviceBusManager = serviceBusManager;
		}

		public async Task CreateAsync(EntityType entityType, string entityPath, params string[] subscribers)
			=> await _serviceBusManager.CreateAsync(_serviceBusConnectionString, entityType, entityPath, subscribers);

		public async Task CreateQueueAsync(string entityPath)
			=> await _serviceBusManager.CreateQueueAsync(_serviceBusConnectionString, entityPath);

		public async Task CreateTopicAsync(string entityPath, params string[] subscribers)
			=> await _serviceBusManager.CreateTopicAsync(_serviceBusConnectionString, entityPath, subscribers);

		public async Task EnviarMensagem(EntityType entityType, string entityPath, string mensagem, TimeSpan? agendarPara = null)
			=> await _serviceBusClient.EnviarMensagem(_serviceBusConnectionString, entityType, entityPath, mensagem, agendarPara);

		public async Task EnviarMensagemParaFila(string entityPath, Message message)
			=> await _serviceBusClient.EnviarMensagemParaFila(_serviceBusConnectionString, entityPath, message);

		public async Task EnviarMensagemParaTopico(string entityPath, Message message)
			=> await _serviceBusClient.EnviarMensagemParaTopico(_serviceBusConnectionString, entityPath, message);
	}
}