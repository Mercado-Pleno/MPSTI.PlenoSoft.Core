using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public static class JsonSerializer
	{
		public static readonly JsonSerializerSettings DefautSetting = new JsonSerializerSettings()
		{
			ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy { OverrideSpecifiedNames = false } },
			DefaultValueHandling = DefaultValueHandling.Include,
			Formatting = Formatting.None,
			NullValueHandling = NullValueHandling.Ignore,
			TypeNameHandling = TypeNameHandling.None
		};
	}
}