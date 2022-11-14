using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using MPSTI.PlenoSoft.Core.Camunda.Services;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Triggers.Timers
{
	public class CamundaFetchExternalTask : CamundaWorker
	{
		private const string Timer1 = "%Camunda_TimerTrigger1%";
		private const string Timer2 = "%Camunda_TimerTrigger2%";
		private const string Timer3 = "%Camunda_TimerTrigger3%";
		private ILogger _logger;

		public CamundaFetchExternalTask(ICamundaClient camundaClient) : base(camundaClient) { }


		[FunctionName("FetchExternalTask_Timer1")]
		public async Task FetchExternalTask_Timer1([TimerTrigger(Timer1)] TimerInfo timerInfo, ILogger logger)
			=> await FetchExternalTaskOnTimer(timerInfo.Schedule.ToString(), logger);


		[FunctionName("FetchExternalTask_Timer2")]
		public async Task FetchExternalTask_Timer2([TimerTrigger(Timer2)] TimerInfo timerInfo, ILogger logger)
			=> await FetchExternalTaskOnTimer(timerInfo.Schedule.ToString(), logger);


		[FunctionName("FetchExternalTask_Timer3")]
		public async Task FetchExternalTask_Timer3([TimerTrigger(Timer3)] TimerInfo timerInfo, ILogger logger)
			=> await FetchExternalTaskOnTimer(timerInfo.Schedule.ToString(), logger);


		private async Task FetchExternalTaskOnTimer(string timerSchedule, ILogger logger)
		{
			_logger = logger;
			await ExternalTaskFetchAndLock($"FetchExternalTaskOnTimer::{timerSchedule}");
		}

		protected override async Task EnqueueExternalTask(ExternalTask externalTask)
			=> await Task.CompletedTask;

		protected override void LogInformation(string message, params object[] args)
			=> _logger.LogInformation(message, args);

		protected override void LogTrace(string message, params object[] args)
			=> _logger.LogTrace(message, args);

		protected override void LogError(Exception exception, string message, params object[] args)
			=> _logger.LogError(exception, message, args);
	}
}