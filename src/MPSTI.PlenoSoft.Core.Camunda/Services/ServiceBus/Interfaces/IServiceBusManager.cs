using Microsoft.Azure.WebJobs.ServiceBus;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Camunda.Services.ServiceBus.Interfaces
{
	public interface IServiceBusManager
	{
		Task CreateAsync(string connectionString, EntityType entityType, string entityPath, params string[] subscribers);

		Task CreateQueueAsync(string connectionString, string entityPath);

		Task CreateTopicAsync(string connectionString, string entityPath, params string[] subscribers);
	}
}