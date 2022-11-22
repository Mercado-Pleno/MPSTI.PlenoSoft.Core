using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages
{
	public class Message
	{
		[JsonProperty("messageName")]
		public string MessageName { get; set; }

		[JsonProperty("businessKey")]
		public string BusinessKey { get; set; }

		[JsonProperty("processInstanceId")]
		public string ProcessInstanceId { get; set; }

		[JsonProperty("correlationKeys")]
		public Variables CorrelationKeys { get; set; }

		[JsonProperty("processVariables")]
		public Variables ProcessVariables { get; set; }

		[JsonProperty("resultEnabled")]
		public bool ResultEnabled { get; set; }
	}
}