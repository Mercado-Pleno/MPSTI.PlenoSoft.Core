using Microsoft.Extensions.Configuration;

namespace MPSTI.PlenoSoft.Core.Extensions.Static
{
	public static class Configurations
	{

		public static TConfigObject GetFromSection<TConfigObject>(this IConfiguration configuration, string sectionKey) where TConfigObject : new()
			=> configuration.GetRequiredSection(sectionKey).Get<TConfigObject>();

		public static TConfigObject GetFromSection<TConfigObject>(this IConfiguration configuration, string sectionKey, TConfigObject defaultConfigObject) where TConfigObject : new()
		{
			var section = configuration.GetSection(sectionKey);
			return section.Exists() ? section.Get<TConfigObject>() : defaultConfigObject;
		}

		public static TConfigObject Get<TConfigObject>(this IConfigurationSection section) where TConfigObject : new()
		{
			var configObject = new TConfigObject();
			section.Bind(configObject);
			return configObject;
		}
	}
}