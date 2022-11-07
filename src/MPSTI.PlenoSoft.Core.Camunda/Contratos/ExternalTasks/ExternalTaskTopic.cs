using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks
{
	public class ExternalTaskTopic
	{
		[JsonProperty("topicName"), JsonPropertyName("topicName")]
		public string TopicName { get; set; }

		[JsonProperty("lockDuration"), JsonPropertyName("lockDuration")]
		public long LockDuration { get; set; }

		[JsonProperty("variables"), JsonPropertyName("variables")]
		public List<string> Variables { get; set; }
	}
}