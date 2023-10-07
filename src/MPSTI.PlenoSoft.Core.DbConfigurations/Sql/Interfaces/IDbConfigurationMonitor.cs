using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces
{
	public delegate void DbConfigurationChangeEventHandler(IDictionary<string, string> newData);

	public interface IDbConfigurationMonitor
    {
        void VerifyChanges(IDictionary<string, string> currentData);
    }
}