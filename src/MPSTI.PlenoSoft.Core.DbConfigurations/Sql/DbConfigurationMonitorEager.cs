using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;
using System.Timers;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public class DbConfigurationMonitorEager : DbConfigurationMonitor
	{
		private readonly IDictionary<string, string> _currentData;
		private readonly Timer _checkChangeTimer;

		public DbConfigurationMonitorEager(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval, DbConfigurationChangeEventHandler onChange)
			: base(dbConfigurationSource, checkChangeInterval, onChange)
		{
			_currentData = new Dictionary<string, string>();
			_checkChangeTimer = new Timer { AutoReset = false, Enabled = false };
			_checkChangeTimer.Elapsed += (s, e) => CheckChanges(_currentData);
			_onChange += VerifyChanges;
		}

		public override void VerifyChanges(IDictionary<string, string> currentData)
		{
			currentData.CopyTo(_currentData);
			if (Enabled)
			{
				var interval = GetInterval(_currentData) ?? _checkChangeInterval;
				_checkChangeTimer.Interval = interval.TotalMilliseconds;
				_checkChangeTimer.Start();
			}
		}
	}
}