using MPSTI.PlenoSoft.Core.CrossProject.Collections;
using MPSTI.PlenoSoft.Core.CrossProject.Configurations;
using MPSTI.PlenoSoft.Core.CrossProject.Connections.Dialects;
using MPSTI.PlenoSoft.Core.CrossProject.Utils;
using System;
using System.IO;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public interface IConnectionManagerFactory : IDisposable
	{
		IConnectionManager CreateConnectionManager();
	}

	public enum Provider { MySql, SqlServer, SQLiteSystem, SQLiteMicrosoft }

	public abstract class AbstractConnectionManagerFactory : IConnectionManagerFactory
	{
		protected AbstractConnectionManagerFactory() { }

		protected abstract Database GetConfig();

		public virtual IConnectionManager CreateConnectionManager()
		{
			var database = GetConfig();

			return database?.Provider switch
			{
				Provider.MySql => CreateConnectionManagerForMySql(database.ConnectionString),
				Provider.SqlServer => CreateConnectionManagerForSqlServer(database.ConnectionString),
				Provider.SQLiteSystem => CreateConnectionManagerForSQLiteSystem(database.ConnectionString),
				Provider.SQLiteMicrosoft => CreateConnectionManagerForSQLiteMicrosoft(database.ConnectionString),

				_ => OnCreateConnectionManager(database?.ConnectionString)
			};
		}

		protected virtual IConnectionManager OnCreateConnectionManager(string connectionString) => null;

		protected virtual IConnectionManager CreateConnectionManagerForSqlServer(string connectionString)
		{
			return new ConnectionManager<SqlServerDialect>(connectionString);
		}

		protected virtual IConnectionManager CreateConnectionManagerForSQLiteSystem(string connectionString)
		{
			return new ConnectionManager<SQLiteSystemDialect>(connectionString);
		}

		protected virtual IConnectionManager CreateConnectionManagerForSQLiteMicrosoft(string connectionString)
		{
			return new ConnectionManager<SqliteMicrosoftDialect>(connectionString);
		}

		protected virtual IConnectionManager CreateConnectionManagerForMySql(string connectionString)
		{
			return new ConnectionManager<MySqlDialect>(connectionString);
		}

		public virtual void Dispose() { }

		protected FileInfo GetTemplateDatabaseFileInfo(string databaseFile)
		{
			var database = new FileInfo(databaseFile);
			if (!database.Exists)
			{
				var directory = database.Directory;
				var template = Path.Combine(directory.FullName, "Template", database.Name);
				var databaseTemplate = new FileInfo(template);
				if (databaseTemplate.Exists)
				{
					directory.GetFiles().ForEach(f => f.TryDeleteIfExists());
					databaseTemplate.CopyTo(database.FullName, true);
				}
			}

			return database;
		}
	}
}