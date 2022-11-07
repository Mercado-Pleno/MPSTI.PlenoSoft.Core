using Microsoft.Azure.WebJobs.ServiceBus;
using MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus
{
	public class ServiceBusManager : IServiceBusManager
	{
		public virtual async Task CreateQueueAsync(string connectionString, string entityPath)
		{
			var serviceBusManager = new ServiceBusManagerClient(connectionString);
			await serviceBusManager.CreateQueueAsync(entityPath);
		}

		public virtual async Task CreateTopicAsync(string connectionString, string entityPath, params string[] subscribers)
		{
			var serviceBusManager = new ServiceBusManagerClient(connectionString);
			await serviceBusManager.CreateTopicAsync(entityPath, subscribers);
		}

		public virtual async Task CreateAsync(string connectionString, EntityType entityType, string entityPath, params string[] subscribers)
		{
			if (entityType == EntityType.Queue)
				await CreateQueueAsync(connectionString, entityPath);
			else
				await CreateTopicAsync(connectionString, entityPath, subscribers);
		}
	}
}