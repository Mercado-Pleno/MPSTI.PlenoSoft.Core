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
		private const string Timer1 = "%Camunda_TimerTrigger_Dia%";
		private const string Timer2 = "%Camunda_TimerTrigger_Noi%";
		private const string Timer3 = "%Camunda_TimerTrigger_Mad%";

		public CamundaFetchExternalTask(ICamundaClient camundaClient) : base(camundaClient) { }


		[FunctionName("FetchExternalTask_Timer1")]
		public async Task FetchExternalTask_Timer1([TimerTrigger(Timer1)] TimerInfo t, ILogger l)
			=> await FetchExternalTaskOnTimer(t, l);


		[FunctionName("FetchExternalTask_Timer2")]
		public async Task FetchExternalTask_Timer2([TimerTrigger(Timer2)] TimerInfo t, ILogger l)
			=> await FetchExternalTaskOnTimer(t, l);


		[FunctionName("FetchExternalTask_Timer3")]
		public async Task FetchExternalTask_Timer3([TimerTrigger(Timer3)] TimerInfo t, ILogger l)
			=> await FetchExternalTaskOnTimer(t, l);


		private async Task FetchExternalTaskOnTimer(TimerInfo timer, ILogger log)
		{
			log.LogInformation($"{timer.Schedule}: {DateTime.Now}");
			await Task.CompletedTask;
		}

		protected override async Task EnqueueExternalTask(ExternalTask externalTask)
		{
			await Task.CompletedTask;
		}
	}
}