using MPSTI.PlenoSoft.Core.Camunda.Contracts;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskComplete
	{
		[JsonProperty("workerId"), JsonPropertyName("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("variables"), JsonPropertyName("variables")]
		public Variables Variables { get; set; }

		public ExternalTaskComplete(ExternalTask task, Variables variables)
		{
			WorkerId = task.WorkerId;
			Variables = variables ?? new Variables();
		}
	}
}