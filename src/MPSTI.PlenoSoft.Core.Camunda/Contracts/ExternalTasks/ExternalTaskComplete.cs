using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskComplete
	{
		[JsonProperty("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("variables")]
		public Variables Variables { get; set; }

		public ExternalTaskComplete(ExternalTask task, Variables variables)
		{
			WorkerId = task.WorkerId;
			Variables = variables ?? new Variables();
		}
	}
}