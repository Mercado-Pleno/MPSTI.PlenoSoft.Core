using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System.Data;
using System.Data.SqlClient;

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
				.AddDbConfiguration<SqliteConfigurationSettings>()
				.AddDbConfiguration(x =>
				{
					x.CommandSelectQuerySql = "Select * From Configuracao";
					x.ConfigurationKeyColumn = "Key";
					x.ConfigurationValueColumn = "Value";
					x.DbConnectionFactory = configuration => new SqliteConnection(configuration.GetConnectionString("Sqlite"));
				})
			;
		}

		[Fact]
		public void Test()
		{
			var configuration = _configurationBuilder.Build();

			configuration.Should().NotBeNull();
			configuration.Providers.Should().NotBeNull();
			configuration.Providers.Should().HaveCount(3);
		}
	}

	public class SqlServerConfigurationSettings : IDbConfigurationSettings
	{
		public string CommandSelectQuerySql => "Select Key, Value From Configuracao";
		public string ConfigurationKeyColumn => "Key";
		public string ConfigurationValueColumn => "Value";
		public IDbConnection CreateDbConnection(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("SqlServer-ConnectionName");
			return new SqlConnection(connectionString);
		}
	}

	public class SqliteConfigurationSettings : DbConfigurationSettings
	{
		public override string CommandSelectQuerySql => "Select Key, Value From Configuracao";
		public override string ConfigurationKeyColumn => "Key";
		public override string ConfigurationValueColumn => "Value";
		public override IDbConnection CreateDbConnection(IConfiguration configuration)
		{
			return new SqliteConnection(configuration.GetConnectionString("Sqlite"));
		}
	}
}