using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPSTI.PlenoSoft.Camunda.Services.Extensions
{
	public static class ConfigurationExtensions
	{
		public static TConfigObject Get<TConfigObject>(this IConfiguration configuration, string sectionKey) where TConfigObject : new()
		 => configuration.GetSection(sectionKey).Get<TConfigObject>();

		public static TConfigObject Get<TConfigObject>(this IConfigurationSection configurationSection) where TConfigObject : new()
			=> JsonConvert.DeserializeObject<TConfigObject>(configurationSection.Value);

	}
}