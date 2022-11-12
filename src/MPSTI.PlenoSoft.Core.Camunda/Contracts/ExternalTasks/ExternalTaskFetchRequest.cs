using Newtonsoft.Json;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskFetchRequest
	{
		[JsonProperty("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("maxTasks")]
		public int MaxTasks { get; set; }

		[JsonProperty("topics")]
		public List<ExternalTaskTopic> Topics { get; set; }
	}
}