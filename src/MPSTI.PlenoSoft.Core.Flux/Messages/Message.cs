using System;
using System.Runtime.Serialization;

namespace MPSC.PlenoSoft.ControlFlux.Messages
{
	[Serializable, DataContract]
	public class Message
	{
		private static Int64 _messageCode = 0L;
		public Guid Id { get; } = Guid.NewGuid();
		public DateTime Created { get; } = DateTime.UtcNow;
		public Int64 MessageCode { get; set; }
		public MessageType Type { get; set; }
		public String Description { get; set; }
		public String FullDescription { get; set; }

		public Message(Int64? messageCode = null)
		{
			MessageCode = messageCode ?? (++_messageCode);
		}

		public override String ToString()
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