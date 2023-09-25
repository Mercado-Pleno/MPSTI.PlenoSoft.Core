using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public class DbConfigurationSource : IDbConfigurationSource
	{
		private readonly IDbConfigurationSettings _dbConfigurationSettings;
		private IConfigurationBuilder _configurationBuilder;

		public DbConfigurationSource(IDbConfigurationSettings dbConfigurationSettings)
		{
			_dbConfigurationSettings = dbConfigurationSettings;
		}

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			_configurationBuilder = builder;
			return new DbConfigurationProvider(this);
		}

		public void FillDataSource(IDictionary<string, string> dataSource)
		{
			using var configuration = _configurationBuilder.BuildPartialConfiguration();
			using var dbConnection = GetConnection(configuration);
			try
			{
				using var dbCommand = dbConnection.CreateCommand(_dbConfigurationSettings.CommandSelectQuerySql);
				using var dataReader = dbCommand.ExecuteReader();
				try
				{
					var keyIndex = dataReader.GetOrdinal(_dbConfigurationSettings.ConfigurationKeyColumn);
					var valueIndex = dataReader.GetOrdinal(_dbConfigurationSettings.ConfigurationValueColumn);
					while (dataReader.Read())
					{
						var key = dataReader.GetString(keyIndex);
						var value = dataReader.GetString(valueIndex);
						dataSource[key] = value;
					}
				}
				finally
				{
					dataReader.Close();
				}
			}
			finally
			{
				dbConnection.Close();
			}
		}

		private IDbConnection GetConnection(IConfiguration configuration)
		{
			var dbConnection = _dbConfigurationSettings.CreateDbConnection(configuration);
			if (dbConnection.State != ConnectionState.Open)
				dbConnection.Open();
			return dbConnection;
		}

		public override string ToString() => _dbConfigurationSettings.ToString();
	}
}