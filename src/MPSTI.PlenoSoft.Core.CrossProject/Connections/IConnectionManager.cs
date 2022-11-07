using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public interface IConnectionManager : IDisposable
	{
		void Open();
		void Close();

		IDbConnection Connection { get; }
		IDbTransaction Transaction { get; }
		IConnectionDialect Dialect { get; }

		IDbTransaction BeginTransaction();
		void EndTransaction(bool commit);

		void SetConnectionManager(INeedConnectionManager connectionManagerSetter);

		TClass CreateInstance<TClass>(Action<TClass> setup = null) where TClass : new();

		TClass CreateInstance<TClass>(Action<TClass> setup = null, params object[] parametros) where TClass : class;
	}
}