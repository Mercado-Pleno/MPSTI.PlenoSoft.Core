using MPSTI.PlenoSoft.Camunda.Extensions;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Contratos.ExternalTasks
{
	public class ExternalTaskError
	{
		[JsonProperty("workerId"), JsonPropertyName("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("errorCode"), JsonPropertyName("errorCode")]
		public string ErrorCode { get; set; }

		[JsonProperty("errorMessage"), JsonPropertyName("errorMessage")]
		public string ErrorMessage { get; set; }

		[JsonProperty("variables"), JsonPropertyName("variables")]
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