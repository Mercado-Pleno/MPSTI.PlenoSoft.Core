using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using MPSTI.PlenoSoft.Core.CrossProject.Connections.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace MPSTI.PlenoSoft.Core.CrossProject.Queries
{
	public abstract class Query : INeedConnectionManager
	{
		protected IConnectionManager ConnectionManager { get; set; }

		IConnectionManager INeedConnectionManager.ConnectionManager => ConnectionManager;

		private int index = 0;
		protected virtual IEnumerable<TResult> ExecuteReader<TResult>(string cmdSQL, IDictionary<string, object> parameters, Func<IDataRecord, TResult> func)
		{
			var iDbCommand = ConnectionManager?.CreateCommand(cmdSQL, ConnectionManager?.Transaction);
			iDbCommand?.CreateParameter(parameters);
			return iDbCommand?.ExecuteReader(func);
		}

		protected virtual long ExecuteScalar(string cmdSQL, IDictionary<string, object> parameters = null)
		{
			using var iDbCommand = ConnectionManager?.CreateCommand(cmdSQL, ConnectionManager?.Transaction);
			iDbCommand?.CreateParameter(parameters);
			var retorno = iDbCommand?.ExecuteScalar();
			return Convert.ToInt64(retorno);
		}

		protected virtual int? ExecuteNonQuery(string cmdSQL, IDictionary<string, object> parameters = null)
		{
			using var iDbCommand = ConnectionManager?.CreateCommand(cmdSQL, ConnectionManager?.Transaction);
			iDbCommand?.CreateParameter(parameters);
			return iDbCommand?.ExecuteNonQuery();
		}

		protected int IndexOf(string[] array, string key)
		{
			if (index < array.Length && array[index] == key)
				return index++;

			index = 0;

			while (index < array.Length && array[index] != key)
				index++;

			return index < array.Length && array[index] == key ? index++ : -1;
		}

		public void SetConnectionManager(IConnectionManager connectionManager)
		{
			ConnectionManager = connectionManager;
		}
	}
}