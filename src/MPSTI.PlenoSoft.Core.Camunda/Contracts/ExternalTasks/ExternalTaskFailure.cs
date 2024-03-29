﻿using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using Newtonsoft.Json;
using System;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts.ExternalTasks
{
	public class ExternalTaskFailure
	{
		[JsonProperty("workerId")]
		public string WorkerId { get; set; }

		[JsonProperty("errorMessage")]
		public string ErrorMessage { get; set; }

		[JsonProperty("errorDetails")]
		public string ErrorDetails { get; set; }

		[JsonProperty("retries")]
		public string Retries { get; set; }

		[JsonProperty("retryTimeout")]
		public string RetryTimeout { get; set; }

		[JsonProperty("errorCode")]
		public string ErrorCode { get; set; }

		[JsonProperty("variables")]
		public Variables Variables { get; set; }

		public ExternalTaskFailure(ExternalTask task, string message, Variables variables = null)
		{
			ErrorCode = "erroExternalTask";
			WorkerId = task.WorkerId;
			Variables = variables ?? new Variables();
			ErrorMessage = message ?? "";
		}

		public ExternalTaskFailure(ExternalTask task, Exception exception, Variables variables = null) : this(task, exception?.Messages("\r\n- Exception: "), variables) { }

		public ExternalTaskFailure(ExternalTask task, Variables variables = null) : this(task, string.Empty, variables) { }
	}
}