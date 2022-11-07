using System;
using System.Runtime.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Extensions
{
	[Serializable]
	public class CamundaException : Exception
	{
		public CamundaException() { }

		protected CamundaException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }

		public CamundaException(string message, Exception innerException) : base(message, innerException) { }
	}
}