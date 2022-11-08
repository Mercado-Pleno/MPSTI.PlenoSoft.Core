using Microsoft.Extensions.Configuration;
using System.IO;

namespace MPSTI.PlenoSoft.Core.xUnit.Factories
{
	public static class ConfigurationFactory
	{
		public static readonly IConfiguration Configuration = Create();

		private static IConfiguration Create()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true, true)
				.Build();
		}
	}
}