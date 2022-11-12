using Newtonsoft.Json;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskTopic
	{
		[JsonProperty("topicName")]
		public string TopicName { get; set; }

		[JsonProperty("lockDuration")]
		public long LockDuration { get; set; }

		[JsonProperty("variables")]
		public List<string> Variables { get; set; }
	}
}