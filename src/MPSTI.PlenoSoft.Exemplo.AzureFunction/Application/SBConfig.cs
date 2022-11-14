using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Services;
using MPSTI.PlenoSoft.Core.Camunda.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Application
{
	public static class SBConfig
	{
		public const string ConnectionName = "Azure_ServiceBus";
		public const string SubscriberName = "PlenoSoft-AzureFunctionTeste";

		public class Camunda
		{
			protected Camunda() { }
			public const string CriaAssinatura = "Cobranca/Cria.Assinatura";
		}

		public class MagPag
		{
			protected MagPag() { }
		}

		public static void Setup(IConfiguration configuration) => SetupAsync(configuration).Wait();

		public static async Task SetupAsync(IConfiguration configuration)
		{
			var serviceBusConnectionString = configuration.GetValue<string>(ConnectionName);
			var serviceBusManagerClient = new ServiceBusManagerClient(serviceBusConnectionString);

			var camundaQueues = GetConstantValues<Camunda>();
			var topics = Array.Empty<string>();

			await ExternalTaskConfig.ConfigureCamundaTopics(TimeSpan.FromMinutes(2), camundaQueues);
			await serviceBusManagerClient.ConfigureServiceBusAsync(camundaQueues, topics, SubscriberName);
		}

		[DebuggerNonUserCode]
		private static async Task ConfigureServiceBusAsync(this IServiceBusManagerClient serviceBusManagerClient, IEnumerable<string> queueNames, IEnumerable<string> topicNames, params string[] subscribers)
		{
			foreach (var queueName in queueNames)
				await serviceBusManagerClient.CreateQueueAsync(queueName);

			foreach (var topicName in topicNames)
				await serviceBusManagerClient.CreateTopicAsync(topicName, subscribers);
		}

		[DebuggerNonUserCode]
		private static string[] GetConstantValues<TClass>()
		{
			var serviceBusClass = typeof(TClass);
			var fields = serviceBusClass.GetFields();
			return fields.Select(a => a.GetValue(0).ToString()).ToArray();
		}
	}
}