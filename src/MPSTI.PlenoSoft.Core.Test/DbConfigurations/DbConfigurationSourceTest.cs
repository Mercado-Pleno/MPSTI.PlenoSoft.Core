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
				.AddJsonFile("Abstracao/appsettings.json", optional: false)
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











































	// .AddDbConfiguration<SqlServerConfigurationSettings>()
	public class SqlServerConfigurationSettings : IDbConfigurationSettings
	{
		public string CommandSelectQuerySql => "Select * From ControleConfiguracaoNegocio Where (Modulo Like '%VG%'). ";
		public string ConfigurationKeyColumn => "Identificador";
		public string ConfigurationValueColumn => "Valor";
		public IDbConnection CreateDbConnection(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("SqlServer");
			return new SqlConnection(connectionString);
		}
	}
}