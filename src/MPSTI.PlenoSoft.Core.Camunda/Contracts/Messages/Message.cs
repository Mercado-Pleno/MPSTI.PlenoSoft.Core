using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages
{
	public class Message
	{
		[JsonProperty("messageName")]
		public string MessageName { get; set; }

		[JsonProperty("businessKey")]
		public string BusinessKey { get; set; }

		[JsonProperty("tenantId")]
		public string TenantId { get; set; }

		[JsonProperty("withoutTenantId")]
		public bool WithoutTenantId { get; set; }

		[JsonProperty("processInstanceId")]
		public string ProcessInstanceId { get; set; }

		[JsonProperty("correlationKeys")]
		public Variables CorrelationKeys { get; set; }

		[JsonProperty("localCorrelationKeys")]
		public Variables LocalCorrelationKeys { get; set; }

		[JsonProperty("processVariables")]
		public Variables ProcessVariables { get; set; }

		[JsonProperty("processVariablesLocal")]
		public Variables ProcessVariablesLocal { get; set; }

		[JsonProperty("all")]
		public bool All { get; set; }

		[JsonProperty("resultEnabled")]
		public bool ResultEnabled { get; set; }

		[JsonProperty("variablesInResultEnabled")]
		public bool VariablesInResultEnabled { get; set; }
	}
}