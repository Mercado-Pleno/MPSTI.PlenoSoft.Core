using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
	public class DbConfigurationMonitorLazy : DbConfigurationMonitor
	{
		private DateTime _lastCheck;
		private DateTime NextCheck => _lastCheck.Add(_checkChangeInterval);

		public DbConfigurationMonitorLazy(IDbConfigurationSource dbConfigurationSource, TimeSpan checkChangeInterval, DbConfigurationChangeEventHandler onChange)
			: base(dbConfigurationSource, checkChangeInterval, onChange)
		{
			_onChange += OnChangeInternal;
		}

		public override void VerifyChanges(IDictionary<string, string> currentData)
		{
			if (Enabled)
			{
				var agora = DateTime.UtcNow;
				if (agora >= NextCheck)
				{
					_lastCheck = agora;
					CheckChanges(currentData);
				}
			}
		}

		private void OnChangeInternal(IDictionary<string, string> newData)
		{
			_checkChangeInterval = GetInterval(newData) ?? _checkChangeInterval;
		}
	}
}