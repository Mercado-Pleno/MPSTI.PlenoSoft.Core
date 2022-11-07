using MPSTI.PlenoSoft.Camunda.Contratos;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks
{
	public class ExternalTask
	{
		[JsonProperty("id"), JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonProperty("workerId"), JsonPropertyName("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("topicName"), JsonPropertyName("topicName")]
		public string TopicName { get; set; }

		[JsonProperty("variables"), JsonPropertyName("variables")]
		public Variables Variables { get; set; }

		[JsonProperty("businessKey"), JsonPropertyName("businessKey")]
		public string BusinessKey { get; set; }
	}
}