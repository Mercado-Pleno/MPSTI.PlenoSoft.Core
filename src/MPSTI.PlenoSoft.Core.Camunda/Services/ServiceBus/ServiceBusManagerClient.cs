using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.WebJobs.ServiceBus;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus
{
	public class ServiceBusManagerClient
	{
		private readonly ManagementClient _serviceBusManagement;

		public ServiceBusManagerClient(string serviceBus_Administrative_ConnectionString)
		{
			_serviceBusManagement = new ManagementClient(serviceBus_Administrative_ConnectionString);
		}

		public virtual async Task CreateAsync(EntityType entityType, string entityPath, params string[] subscribers)
		{
			if (entityType == EntityType.Queue)
				await CreateQueueAsync(entityPath);
			else
				await CreateTopicAsync(entityPath, subscribers);
		}

		#region // "Queue"
		public virtual async Task DeleteQueueAsync(string queuePath)
		{
			if (await _serviceBusManagement.QueueExistsAsync(queuePath))
				await _serviceBusManagement.DeleteQueueAsync(queuePath);
		}

		public virtual async Task CreateQueueAsync(string queuePath)
		{
			if (!await _serviceBusManagement.QueueExistsAsync(queuePath))
			{
				var queueDescription = ServiceBusTemplate.GetQueueDescription(queuePath);
				await _serviceBusManagement.CreateQueueAsync(queueDescription);
			}
		}

		public virtual async Task ResetQueueAsync(string queuePath)
		{
			await DeleteQueueAsync(queuePath);
			await CreateQueueAsync(queuePath);
		}
		#endregion // "Queue"

		#region // "Topic"
		public virtual async Task DeleteTopicAsync(string topicPath)
		{
			if (await _serviceBusManagement.TopicExistsAsync(topicPath))
				await _serviceBusManagement.DeleteTopicAsync(topicPath);
		}

		public virtual async Task CreateTopicAsync(string topicPath, params string[] subscribers)
		{
			if (!await _serviceBusManagement.TopicExistsAsync(topicPath))
			{
				var topicDescription = ServiceBusTemplate.GetTopicDescription(topicPath);
				await _serviceBusManagement.CreateTopicAsync(topicDescription);
			}

			await CreateSubscribersAsync(topicPath, subscribers);
		}

		public virtual async Task CreateSubscribersAsync(string topicPath, params string[] subscribers)
		{
			foreach (var subscriber in subscribers)
				await CreateSubscriberAsync(topicPath, subscriber);
		}

		private async Task CreateSubscriberAsync(string topicPath, string subscriber)
		{
			if (!await _serviceBusManagement.SubscriptionExistsAsync(topicPath, subscriber))
				await _serviceBusManagement.CreateSubscriptionAsync(topicPath, subscriber);
		}

		public virtual async Task ResetTopicAsync(string topicPath)
		{
			await DeleteTopicAsync(topicPath);
			await CreateTopicAsync(topicPath);
		}
		#endregion // "Topic"
	}
}