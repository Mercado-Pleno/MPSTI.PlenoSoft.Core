using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.Configurations.DbSql.Interfaces
{
    public interface IDbConfigurationSource : IConfigurationSource
    {
        void ExecuteQueryAndLoadData(IDictionary<string, string> dataSource);
    }
}