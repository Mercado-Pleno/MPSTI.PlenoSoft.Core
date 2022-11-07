using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.CrossProject.Results
{
	public enum MessageType
	{
		[JsonProperty("internal")]
		Internal = 0,

		[JsonProperty("information")]
		Information = 100,

		[JsonProperty("success")]
		Success = 200,

		[JsonProperty("warning")]
		Warning = 400,

		[JsonProperty("error")]
		Error = 500
	}
}