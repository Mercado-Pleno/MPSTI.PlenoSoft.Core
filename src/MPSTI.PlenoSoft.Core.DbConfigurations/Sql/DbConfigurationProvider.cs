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
		private readonly IDbConfigurationMonitor _dbConfigurationMonitor;

		public DbConfigurationProvider(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval)
		{
			_dbConfigurationSource = dbConfigurationSource;
			_dbConfigurationMonitor = new DbConfigurationMonitorLazy(_dbConfigurationSource, checkChangeInterval, ReloadChanges);
		}

		public override void Load()
		{
			_dbConfigurationSource.FillDataSource(Data);
			_dbConfigurationMonitor.VerifyChanges(Data);
			OnReload();
		}

		public override bool TryGet(string key, out string value)
		{
			_dbConfigurationMonitor.VerifyChanges(Data);
			return base.TryGet(key, out value);
		}

		public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
		{
			_dbConfigurationMonitor.VerifyChanges(Data);
			return base.GetChildKeys(earlierKeys, parentPath);
		}

		private void ReloadChanges(IDictionary<string, string> newData)
		{
			newData.CopyTo(Data);
			OnReload();
		}

		public override string ToString() => $"DbConfigurationProvider for {_dbConfigurationSource}";
	}
}