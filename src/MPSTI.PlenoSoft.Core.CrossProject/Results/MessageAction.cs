using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.CrossProject.Results
{
	public enum MessageAction
	{
		[JsonProperty("none")]
		None = 0,

		[JsonProperty("show")]
		Show = 1,

		[JsonProperty("open")]
		Open = 2,

		[JsonProperty("navigate")]
		Navigate = 4,

		[JsonProperty("download")]
		Download = 8
	}
}