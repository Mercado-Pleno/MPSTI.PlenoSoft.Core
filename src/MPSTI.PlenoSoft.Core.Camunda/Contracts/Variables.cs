using MPSTI.PlenoSoft.Core.Camunda.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPSTI.PlenoSoft.Core.Camunda.Contracts
{
	[Serializable]
	public class Variables : Dictionary<string, Variable>
	{
		public Variables() { }

		protected Variables(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }

		public Variables Add<T>(string name, T value, string type = null)
		{
			base.Add(name, new Variable(value, type ?? CamundaTypes.GetCamundaType(value)));
			return this;
		}

		public T Get<T>(string variableName, T defaultValue = default)
		{
			if (TryGetValue(variableName, out var variable))
				return variable.Get(defaultValue);

			return defaultValue;
		}

		public TJson GetObjectFromJson<TJson>(string variableName, TJson defaultValue = default)
		{
			if (TryGetValue(variableName, out var variable))
				return variable.GetObjectFromJson(defaultValue);

			return defaultValue;
		}
	}
}