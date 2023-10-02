using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public delegate void DbConfigurationChangeEventHandler(IDictionary<string, string> newData);

	public class DbConfigurationMonitor
	{
		private const string _configurationKey = "DbConfigurationMonitor_CheckChangeInterval";
		private readonly static IFormatProvider enUS = new CultureInfo("en-US");

		private readonly IDbConfigurationSource _dbConfigurationSource;
		private readonly IDictionary<string, string> _currentData;
		private readonly TimeSpan _checkChangeInterval;
		private readonly Timer _checkChangeTimer;

		private event DbConfigurationChangeEventHandler _onChange;

		public DbConfigurationMonitor(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval, DbConfigurationChangeEventHandler onChange)
		{
			_dbConfigurationSource = dbConfigurationSource;
			_checkChangeInterval = checkChangeInterval;
			_onChange += onChange;
			_currentData = new Dictionary<string, string>();
			_checkChangeTimer = new Timer { AutoReset = false, Enabled = false };
			_checkChangeTimer.Elapsed += CheckChanges;
		}

		public void Start(IDictionary<string, string> newData)
		{
			newData.CopyTo(_currentData);
			if (_checkChangeInterval.TotalSeconds >= 1)
			{
				var interval = GetInterval(_currentData) ?? _checkChangeInterval;
				_checkChangeTimer.Interval = interval.TotalMilliseconds;
				_checkChangeTimer.Start();
			}
		}

		private void CheckChanges(object sender, ElapsedEventArgs eventArgs)
		{
			var newData = new Dictionary<string, string>();
			try
			{
				_dbConfigurationSource.FillDataSource(newData);

				if (_currentData.HasChanged(newData))
					_onChange?.Invoke(newData);
			}
			finally
			{
				Start(newData);
			}
		}

		private static TimeSpan? GetInterval(IDictionary<string, string> data)
		{
			return data.TryGetValue(_configurationKey, out var configValue) &&
				TimeSpan.TryParse(configValue, enUS, out var interval) ? interval : null;
		}
	}
}