using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Azure.ServiceBus.Interfaces;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;
using MPSTI.PlenoSoft.Core.Camunda.Interfaces;
using MPSTI.PlenoSoft.Core.Camunda.Services;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts;
using MPSTI.PlenoSoft.Exemplo.AzureFunction.Repository;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Triggers.Timers
{
	public class CamundaFetchExternalTask : CamundaWorker
	{
		private const string Timer1 = "%Camunda_Timer1%";
		private const string Timer2 = "%Camunda_Timer2%";
		private const string Timer3 = "%Camunda_Timer3%";
		private readonly IServiceBusWrapperClient _serviceBusWrapperClient;
		private readonly IAlunoRepository _alunoRepository;
		private ILogger _logger;

		public CamundaFetchExternalTask(ICamundaClient camundaClient, IServiceBusWrapperClient serviceBusWrapperClient, IAlunoRepository alunoRepository) : base(camundaClient)
		{
			_serviceBusWrapperClient = serviceBusWrapperClient;
			_alunoRepository = alunoRepository;
		}


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
			await _alunoRepository.CreateOrUpdate(new Aluno { Id = Guid.NewGuid(), Name = "Bruno", Description = "Fernandes", cpf = "123" });
		}

		protected override async Task EnqueueExternalTask(ExternalTask externalTask)
			=> await Enfileirar(externalTask);

		private async Task Enfileirar(ExternalTask externalTask, int tentativa = 1)
		{
			try
			{
				await _serviceBusWrapperClient.EnviarMensagem(
					entityType: EntityType.Queue,
					entityPath: externalTask.TopicName,
					mensagem: JsonConvert.SerializeObject(externalTask)
				);
			}
			catch (MessagingEntityNotFoundException) when (tentativa <= 1)
			{
				await _serviceBusWrapperClient.CreateQueueAsync(externalTask.TopicName);
				await Enfileirar(externalTask, tentativa + 1);
			}
		}


		protected override void LogInformation(string message, params object[] args)
			=> _logger.LogInformation(message, args);

		protected override void LogTrace(string message, params object[] args)
			=> _logger.LogTrace(message, args);

		protected override void LogError(Exception exception, string message, params object[] args)
			=> _logger.LogError(exception, message, args);
	}
}