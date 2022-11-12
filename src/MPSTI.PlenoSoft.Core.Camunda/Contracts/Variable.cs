﻿using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts
{
	public class Variable
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public object Value { get; set; }

		public Variable() { }
		public Variable(object value, string type)
		{
			Value = CamundaTypes.ConvertToCamundaValue(value);
			Type = type ?? CamundaTypes.GetCamundaType(value);
		}

		public override string ToString() => $"{Type} = {Value}";

		public T Get<T>(T defaultValue)
		{
			return CamundaTypes.ChangeType(Value, defaultValue);
		}

		public TJson GetObjectFromJson<TJson>(TJson defaultValue)
		{
			var jsonString = Value?.ToString();
			return string.IsNullOrWhiteSpace(jsonString) ? defaultValue : JsonConvert.DeserializeObject<TJson>(jsonString);
		}
	}
}