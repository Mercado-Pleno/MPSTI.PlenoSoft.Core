using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public abstract class DbConfigurationMonitor : IDbConfigurationMonitor
	{
		private const string _configurationKey = "DbConfigurationMonitor_CheckChangeInterval";
		private readonly static IFormatProvider enUS = new CultureInfo("en-US");
		private readonly IDbConfigurationSource _dbConfigurationSource;
		protected TimeSpan _checkChangeInterval;
		protected event DbConfigurationChangeEventHandler _onChange;

		public bool Enabled => _checkChangeInterval.TotalSeconds >= 5;

		public DbConfigurationMonitor(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval, DbConfigurationChangeEventHandler onChange)
		{
			_dbConfigurationSource = dbConfigurationSource;
			_checkChangeInterval = checkChangeInterval;
			_onChange += onChange;
		}

		public abstract void VerifyChanges(IDictionary<string, string> currentData);

		protected void CheckChanges(IDictionary<string, string> currentData)
		{
			var newData = new Dictionary<string, string>();
			_dbConfigurationSource.FillDataSource(newData);
			if (currentData.HasChanged(newData))
				NotifyChange(newData);
		}

		protected void NotifyChange(Dictionary<string, string> newData) => _onChange.Invoke(newData);

		protected static TimeSpan? GetInterval(IDictionary<string, string> data)
		{
			return data.TryGetValue(_configurationKey, out var configValue) &&
				TimeSpan.TryParse(configValue, enUS, out var interval) ? interval : null;
		}
	}
}