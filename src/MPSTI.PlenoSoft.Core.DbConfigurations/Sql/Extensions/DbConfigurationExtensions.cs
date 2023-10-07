using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions
{
	public static class DbConfigurationExtensions
	{
		/// <summary>
		///	To use this method,
		///		TDbConfigurationSettings needs to implements the IDbConfigurationSettings
		///		OR
		///		TDbConfigurationSettings needs to inherit the DbConfigurationSettings that already implement the IDbConfigurationSettings
		///		
		/// <code>
		/// var builder = new ConfigurationBuilder()
		///		.SetBasePath(Directory.GetCurrentDirectory())
		///		.AddJsonFile("appsettings.json", optional: false)
		///		.AddDbConfiguration<SqlServerConfigurationSettings>()
		///		.AddDbConfiguration<SqliteConfigurationSettings>();
		/// var configuration = builder.Build();
		/// 
		/// // ...
		/// 
		/// public class SqlServerConfigurationSettings : IDbConfigurationSettings
		/// {
		/// 	public string CommandSelectQuerySql => "Select Key, Value From Configuration";
		/// 	public string ConfigurationKeyColumn => "Key";
		/// 	public string ConfigurationValueColumn => "Value";
		/// 	public IDbConnection CreateDbConnection(IConfiguration configuration)
		/// 	{
		/// 		var connectionString = configuration.GetConnectionString("SqlServer-ConnectionName");
		/// 		return new SqlConnection(connectionString);
		/// 	}
		/// }
		/// 
		/// // ...
		/// 
		/// public class SqliteConfigurationSettings : DbConfigurationSettings
		/// {
		/// 	public override string CommandSelectQuerySql => "Select Key, Value From Configuration";
		/// 	public override string ConfigurationKeyColumn => "Key";
		/// 	public override string ConfigurationValueColumn => "Value";
		/// 	public override IDbConnection CreateDbConnection(IConfiguration configuration)
		/// 	{
		/// 		var connectionString = configuration.GetConnectionString("Sqlite-ConnectionName");
		/// 		return new SqliteConnection(connectionString);
		/// 	}
		/// }
		/// </code>
		/// </summary>
		/// <typeparam name="TDbConfigurationSettings">where TDbConfigurationSettings : IDbConfigurationSettings, new()</typeparam>
		/// <param name="builder">IConfigurationBuilder</param>
		/// <returns>IConfigurationBuilder</returns>
		public static IConfigurationBuilder AddDbConfiguration<TDbConfigurationSettings>(this IConfigurationBuilder builder) where TDbConfigurationSettings : IGetDbConfigurationSettings, new()
		{
			var dbConfigurationSettings = new TDbConfigurationSettings();
			return AddDbConfiguration(builder, dbConfigurationSettings);
		}

		/// <summary>
		/// <code>
		/// var builder = new ConfigurationBuilder()
		///		.SetBasePath(Directory.GetCurrentDirectory())
		///		.AddJsonFile("appsettings.json", optional: false)
		///		.AddDbConfiguration(cfg =>
		///		{
		///			cfg.CommandSelectQuerySql = "Select * From Configuration";
		///			cfg.ConfigurationKeyColumn = "Key";
		///			cfg.ConfigurationValueColumn = "Value";
		///			cfg.DbConnectionFactory = configuration => new SqlConnection(configuration.GetConnectionString("ConnectionName"));
		///		});
		/// var configuration = builder.Build();
		/// </code>
		/// </summary>
		/// <param name="builder">IConfigurationBuilder</param>
		/// <param name="setupSettings">Action<DbConfigurationSettings></param>
		/// <returns>IConfigurationBuilder</returns>
		public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Action<ISetDbConfigurationSettings> setupSettings)
		{
			var dbConfigurationSettings = new DbConfigurationSettings();
			setupSettings.Invoke(dbConfigurationSettings);
			return AddDbConfiguration(builder, dbConfigurationSettings);
		}

		public static IConfigurationBuilder AddDbConfiguration(IConfigurationBuilder builder, IGetDbConfigurationSettings dbConfigurationSettings)
		{
			var dbConfigurationSource = new DbConfigurationSource(dbConfigurationSettings);
			return builder?.Add(dbConfigurationSource);
		}

		public static ConfigurationRoot BuildPartialConfiguration(this IConfigurationBuilder configurationBuilder)
		{
			var configurationSources = configurationBuilder.Sources.Where(x => x is not IDbConfigurationSource);
			var configurationProviders = configurationSources.Select(x => x.Build(configurationBuilder)).ToArray();

			foreach (var configurationProvider in configurationProviders)
				configurationProvider.Load();

			return new ConfigurationRoot(configurationProviders);
		}

		public static bool HasChanged(this IDictionary<string, string> currentData, IDictionary<string, string> newData)
		{
			var changed = currentData.Count != newData.Count;
			changed |= !currentData.Keys.All(k => newData.ContainsKey(k));
			changed |= !currentData.Values.All(v => newData.Values.Contains(v));

			if (!changed)
				foreach (var key in currentData.Keys)
					changed |= currentData[key] != newData[key];

			return changed;
		}

		public static void CopyTo<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IDictionary<TKey, TValue> destination)
		{
			destination.Clear();
			foreach (var item in source)
				destination[item.Key] = item.Value;
		}

		public static IDbCommand CreateCommand(this IDbConnection dbConnection, string commandText)
		{
			var dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandType = CommandType.Text;
			dbCommand.CommandText = commandText;
			dbCommand.CommandTimeout = 10;
			return dbCommand;
		}
	}
}