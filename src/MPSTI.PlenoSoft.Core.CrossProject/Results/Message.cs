using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.CrossProject.Results
{
	public class Message
	{
		[JsonProperty("type")]
		public MessageType Type { get; set; }

		[JsonProperty("action")]
		public MessageAction Action { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
	}
}