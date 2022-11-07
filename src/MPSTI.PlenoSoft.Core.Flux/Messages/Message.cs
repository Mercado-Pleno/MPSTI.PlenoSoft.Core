using System;
using System.Runtime.Serialization;

namespace MPSTI.PlenoSoft.Core.Flux.Messages
{
	[Serializable]
	public class Message
	{
		private static long _messageCode = 0L;
		public Guid Id { get; } = Guid.NewGuid();
		public DateTime Created { get; } = DateTime.UtcNow;
		public long MessageCode { get; set; }
		public MessageType Type { get; set; }
		public string Description { get; set; }
		public string FullDescription { get; set; }

		public Message(long? messageCode = null)
		{
			MessageCode = messageCode ?? ++_messageCode;
		}

		public override string ToString()
		{
			return
				"Type = " + Type
				+ ", Description = " + Description
				+ ", FullDescription = " + FullDescription
				+ ", MessageCode = " + MessageCode
				+ ", Created = " + Created
				+ ", Id = " + Id
			;
		}
	}
}