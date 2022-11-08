using Microsoft.Azure.WebJobs.ServiceBus;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces
{
	public interface IServiceBusManagerClient
	{
		Task CreateAsync(EntityType entityType, string entityPath, params string[] subscribers);
		Task CreateQueueAsync(string queuePath);
		Task CreateSubscribersAsync(string topicPath, params string[] subscribers);
		Task CreateTopicAsync(string topicPath, params string[] subscribers);
		Task DeleteQueueAsync(string queuePath);
		Task DeleteTopicAsync(string topicPath);
		Task ResetQueueAsync(string queuePath);
		Task ResetTopicAsync(string topicPath);
	}
}