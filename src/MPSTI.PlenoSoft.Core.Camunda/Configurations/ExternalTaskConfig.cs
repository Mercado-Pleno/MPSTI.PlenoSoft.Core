using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks;

namespace MPSTI.PlenoSoft.Core.Camunda.Configurations
{
	public class ExternalTaskConfig
	{
		private static readonly List<ExternalTaskTopic> _topics = new();
		public int MaxTasks { get; set; }
		public string WorkerId { get; set; }
		public virtual List<ExternalTaskTopic> Topics => _topics;

		public static async Task<int> ConfigureCamundaTopics(TimeSpan defaultLockDuration, params string[] topicNames)
		{
			var lockDuration = Convert.ToInt64(defaultLockDuration.TotalMilliseconds);
			var topics = topicNames.Select(topicName => new ExternalTaskTopic { LockDuration = lockDuration, TopicName = topicName });
			_topics.AddRange(topics);

			return await Task.FromResult(_topics.Count);
		}

		public ExternalTaskFetchRequest ToExternalTaskFetchRequest()
			=> new ExternalTaskFetchRequest { MaxTasks = MaxTasks, WorkerId = WorkerId, Topics = Topics };
	}
}