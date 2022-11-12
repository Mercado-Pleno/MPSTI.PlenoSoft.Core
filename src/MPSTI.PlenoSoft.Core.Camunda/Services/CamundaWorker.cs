using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Services
{
	public abstract class CamundaWorker
	{
		private readonly ICamundaClient _camundaClient;

		public CamundaWorker(ICamundaClient camundaClient) => _camundaClient = camundaClient;

		protected async Task ExternalTaskFetchAndLock(string functionName)
		{
			try
			{
				if (await _camundaClient.HealthCheck())
				{
					var externalTasks = await _camundaClient.ExternalTaskFetchAndLock();
					LogInformation("{FunctionName} - Process {Count} external tasks", functionName, externalTasks.Count);

					foreach (var externalTask in externalTasks)
					{
						LogTrace("=> Enqueue [{externalTask.Id}] in {externalTask.TopicName}", externalTask.Id, externalTask.TopicName);
						await EnqueueExternalTask(externalTask);
					}
				}
			}
			catch (Exception exception)
			{
				var appException = new Exception($"There was a problem Fetching External Tasks in Camunda!", exception);
				LogError(appException, "{FunctionName} - {exceptionName}", functionName, exception.GetType().Name);
			}
		}

		protected abstract Task EnqueueExternalTask(ExternalTask externalTask);

		protected virtual void LogTrace(string message, params object[] args) { }
		protected virtual void LogInformation(string message, params object[] args) { }
		protected virtual void LogError(Exception exception, string message, params object[] args) { }
	}
}