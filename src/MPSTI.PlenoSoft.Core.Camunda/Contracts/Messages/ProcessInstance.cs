using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.Messages
{
	public class ProcessInstance
	{
		public string ProcessDefinitionName { get; }

		[JsonProperty("variables"), JsonPropertyName("variables")]
		public Variables Variables { get; }

		[JsonProperty("withVariablesInReturn"), JsonPropertyName("withVariablesInReturn")]
		public bool WithVariablesInReturn { get; }

		[JsonProperty("businessKey"), JsonPropertyName("businessKey")]
		public string BusinessKey { get; }

		public ProcessInstance(string processDefinitionName, object businessKey, bool withVariablesInReturn = true)
		{
			Variables = new Variables();
			BusinessKey = businessKey?.ToString() ?? "N/A";
			ProcessDefinitionName = processDefinitionName;
			WithVariablesInReturn = withVariablesInReturn;
		}

		public ProcessInstance Add<T>(string name, T value, string type = null)
		{
			Variables.Add(name, value, type);
			return this;
		}
	}
}