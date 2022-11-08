using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages
{
	public class Message
	{
		[JsonProperty("messageName"), JsonPropertyName("messageName")]
		public string MessageName { get; set; }

		[JsonProperty("businessKey"), JsonPropertyName("businessKey")]
		public string BusinessKey { get; set; }

		[JsonProperty("processInstanceId"), JsonPropertyName("processInstanceId")]
		public string ProcessInstanceId { get; set; }

		[JsonProperty("processVariables"), JsonPropertyName("processVariables")]
		public Variables ProcessVariables { get; set; }

		[JsonProperty("resultEnabled"), JsonPropertyName("resultEnabled")]
		public bool ResultEnabled { get; set; }
	}
}