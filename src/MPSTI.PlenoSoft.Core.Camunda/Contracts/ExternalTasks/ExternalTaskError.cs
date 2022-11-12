using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using Newtonsoft.Json;
using System;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskError
	{
		[JsonProperty("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("errorCode")]
		public string ErrorCode { get; set; }

		[JsonProperty("errorMessage")]
		public string ErrorMessage { get; set; }

		[JsonProperty("variables")]
		public Variables Variables { get; set; }

		public ExternalTaskError(ExternalTask task, string message, Variables variables = null)
		{
			ErrorCode = "erroExternalTask";
			WorkerId = task.WorkerId;
			Variables = variables ?? new Variables();
			ErrorMessage = message ?? "";
		}

		public ExternalTaskError(ExternalTask task, Exception exception, Variables variables = null) : this(task, exception?.Messages(), variables) { }
	}
}