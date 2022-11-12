using Newtonsoft.Json;
using System;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTask
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("topicName")]
		public string TopicName { get; set; }

		[JsonProperty("variables")]
		public Variables Variables { get; set; }

		[JsonProperty("businessKey")]
		public string BusinessKey { get; set; }
	}
}