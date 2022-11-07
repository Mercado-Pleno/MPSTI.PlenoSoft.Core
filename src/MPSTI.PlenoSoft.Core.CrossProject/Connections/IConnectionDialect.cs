using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public interface IConnectionDialect
	{
		IDbConnection CreateConnection(string connectionString);

		string GetCmdSqlLastId();

		string ConvertType(Type type);

		string GetPrimaryKeyTemplate(string tabela);
	}
}