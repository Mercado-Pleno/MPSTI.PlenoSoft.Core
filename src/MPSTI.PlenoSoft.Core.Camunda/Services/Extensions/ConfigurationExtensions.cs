using Microsoft.Extensions.Configuration;

namespace MPSTI.PlenoSoft.Camunda.Services.Extensions
{
	public static class ConfigurationExtensions
	{
		public static TConfigObject Get<TConfigObject>(this IConfiguration configuration, string sectionKey) where TConfigObject : new()
		{
			var configObject = new TConfigObject();
			var section = configuration.GetSection(sectionKey);
			section.Bind(configObject);
			return configObject;
		}
	}
}