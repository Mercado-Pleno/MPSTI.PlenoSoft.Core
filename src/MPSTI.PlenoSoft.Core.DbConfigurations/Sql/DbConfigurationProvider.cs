using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public class DbConfigurationProvider : ConfigurationProvider
	{
		private readonly IDbConfigurationSource _dbConfigurationSource;
		private readonly DbConfigurationMonitor _dbConfigurationMonitor;

		public DbConfigurationProvider(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval)
		{
			_dbConfigurationSource = dbConfigurationSource;
			_dbConfigurationMonitor = new DbConfigurationMonitor(_dbConfigurationSource, checkChangeInterval, Reload);
		}

		public override void Load()
		{
			Data.Clear();
			_dbConfigurationSource.FillDataSource(Data);
			_dbConfigurationMonitor.Start(Data);
			OnReload();
		}

		private void Reload(IDictionary<string, string> newData)
		{
			newData.CopyTo(Data);
			OnReload();
		}

		public override string ToString() => $"DbConfigurationProvider for {_dbConfigurationSource}";
	}
}