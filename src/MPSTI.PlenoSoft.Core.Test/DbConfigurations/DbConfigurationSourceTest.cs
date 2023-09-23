using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql;
using System.Data;

namespace MPSTI.PlenoSoft.Core.Test.DbConfigurations
{
	public class DbConfigurationSourceTest
	{
		private readonly IConfigurationBuilder _configurationBuilder;

		public DbConfigurationSourceTest()
		{
			_configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("Abstracao/appsettings.json", optional: false, reloadOnChange: true)
				.AddDbConfiguration<SqliteConfigurationSource>()
			;
		}

		[Fact]
		public void Test()
		{
			var configuration = _configurationBuilder.Build();

			configuration.Should().NotBeNull();
			configuration.Providers.Should().NotBeNull();
			configuration.Providers.Should().HaveCount(2);
		}
	}

	public class SqliteConfigurationSource : DbConfigurationSource
	{
		protected override string CommandSelectQuerySql => "Select * From Configuracao";
		protected override string ConfigurationKeyColumn => "Key";
		protected override string ConfigurationValueColumn => "Value";
		protected override IDbConnection CreateDbConnection(IConfiguration configuration)
		{
			return new SqliteConnection(configuration.GetConnectionString("Sqlite"));
		}
	}
}