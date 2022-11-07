using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks
{
	public class ExternalTaskFetchRequest
	{
		[JsonProperty("workerId"), JsonPropertyName("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("maxTasks"), JsonPropertyName("maxTasks")]
		public int MaxTasks { get; set; }

		[JsonProperty("topics"), JsonPropertyName("topics")]
		public List<ExternalTaskTopic> Topics { get; set; }
	}
}