using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections.Data
{
	public static class DbExtension
	{
		public static IDbCommand CreateCommand(this IConnectionManager connectionManager, string cmdSQL, IDbTransaction iDbTransaction = null, TimeSpan? commandTimeout = null, CommandType commandType = CommandType.Text)
			=> connectionManager.Connection.CreateCommandImpl(cmdSQL, iDbTransaction, commandTimeout, commandType);

		public static IDbCommand CreateCommand(this IDbConnection iDbConnection, string cmdSql, IDbTransaction iDbTransaction = null, TimeSpan? commandTimeout = null, CommandType commandType = CommandType.Text)
			=> iDbConnection.CreateCommandImpl(cmdSql, iDbTransaction, commandTimeout, commandType);

		public static IDbDataParameter CreateParameter(this IDbCommand iDbCommand, string parameterName, string value, DbType? dbType = null, ParameterDirection parameterDirection = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null)
			=> iDbCommand.CreateParameterImpl(parameterName, value, dbType, parameterDirection, size, precision, scale);

		public static IDbDataParameter CreateParameter(this IDbCommand iDbCommand, string parameterName, object value, DbType? dbType = null, ParameterDirection parameterDirection = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null)
			=> iDbCommand.CreateParameterImpl(parameterName, value, dbType, parameterDirection, size, precision, scale);

		public static IEnumerable<IDbDataParameter> CreateParameter(this IDbCommand iDbCommand, IDictionary<string, object> parameters, ParameterDirection parameterDirection = ParameterDirection.Input)
			=> iDbCommand.CreateParameterDic(parameters, parameterDirection);

		public static IEnumerable<IDbDataParameter> CreateParameter<T>(this IDbCommand iDbCommand, string parameterName, IEnumerable<T> values, DbType? dbType = null, ParameterDirection parameterDirection = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null)
			=> iDbCommand.CreateParameterList(parameterName, values, dbType, parameterDirection, size, precision, scale);

		public static void CreateParameters<T>(this IDbCommand iDbCommand, string parameterRoot, IEnumerable<T> values, DbType? dbType = null, ParameterDirection parameterDirection = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null) where T : struct
		{
			if (values.Count() <= 100)
				iDbCommand.CreateParameterList(parameterRoot, values, dbType, parameterDirection, size, precision, scale);
			else
				iDbCommand.InjectParameterImpl(parameterRoot, values, dbType);
		}

		private static IDbCommand CreateCommandImpl(this IDbConnection iDbConnection, string cmdSql, IDbTransaction iDbTransaction = null, TimeSpan? commandTimeout = null, CommandType commandType = CommandType.Text)
		{
			var iDbCommand = iDbConnection.CreateCommand();
			iDbCommand.CommandText = cmdSql;
			iDbCommand.CommandTimeout = Convert.ToInt32(commandTimeout?.TotalSeconds ?? 60);
			iDbCommand.CommandType = commandType;
			iDbCommand.Transaction = iDbTransaction;
			return iDbCommand;
		}

		private static IDbDataParameter CreateParameterImpl(this IDbCommand iDbCommand, string parameterName, object value, DbType? dbType = null, ParameterDirection? direction = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null)
		{
			var iDbDataParameter = iDbCommand.CreateParameter();
			iDbDataParameter.ParameterName = parameterName;
			iDbDataParameter.Value = value ?? DBNull.Value;
			if (dbType.HasValue) iDbDataParameter.DbType = dbType.Value;
			iDbDataParameter.Direction = direction ?? ParameterDirection.Input;
			iDbDataParameter.Precision = precision ?? iDbDataParameter.Precision;
			iDbDataParameter.Scale = scale ?? iDbDataParameter.Scale;
			iDbDataParameter.Size = size ?? iDbDataParameter.Size;

			iDbCommand.Parameters.Add(iDbDataParameter);
			return iDbDataParameter;
		}

		private static IEnumerable<IDbDataParameter> CreateParameterList<T>(this IDbCommand iDbCommand, string parameterRoot, IEnumerable<T> values, DbType? dbType = null, ParameterDirection parameterDirection = ParameterDirection.Input, int? size = null, byte? precision = null, byte? scale = null)
		{
			var parameterLists = new List<IDbDataParameter>();
			var parameterNames = new List<string>();
			var parameterIndex = 0;

			foreach (var value in values)
			{
				var parameterName = $"{parameterRoot}{++parameterIndex}";
				parameterNames.Add(parameterName);
				parameterLists.Add(iDbCommand.CreateParameterImpl(parameterName, value, dbType, parameterDirection, size, precision, scale));
			}

			iDbCommand.ReplaceParameterImpl(parameterRoot, parameterNames);

			return parameterLists;
		}

		private static IEnumerable<IDbDataParameter> CreateParameterDic(this IDbCommand iDbCommand, IDictionary<string, object> parameters, ParameterDirection parameterDirection = ParameterDirection.Input)
		{
			var parameterLists = new List<IDbDataParameter>();
			if (parameters != null)
			{
				foreach (var parameter in parameters)
					parameterLists.Add(iDbCommand.CreateParameterImpl(parameter.Key, parameter.Value, null, parameterDirection, null, null, null));
			}
			return parameterLists;
		}

		private static bool InjectParameterImpl<T>(this IDbCommand iDbCommand, string parameterName, IEnumerable<T> values, DbType? dbType = null, int? size = null)
		{
			return iDbCommand.ReplaceParameterImpl("@" + parameterName, values.Parse(dbType))
				|| iDbCommand.ReplaceParameterImpl(":" + parameterName, values.Parse(dbType))
				|| iDbCommand.ReplaceParameterImpl(parameterName, values.Parse(dbType));
		}

		private static bool ReplaceParameterImpl<T>(this IDbCommand iDbCommand, string parameterName, IEnumerable<T> parameters)
		{
			var originalCommandText = iDbCommand.CommandText;
			var newCommandText = DbUtilExtension.Replace(iDbCommand.CommandText, parameterName, string.Join(",", parameters));
			iDbCommand.CommandText = newCommandText;

			return !newCommandText.Equals(originalCommandText);
		}

		public static object ExecuteScalar<TResult>(this IDbCommand iDbCommand)
		{
			try
			{
				return iDbCommand.ExecuteScalar();
			}
			finally
			{
				iDbCommand.Dispose();
			}
		}

		public static IEnumerable<TResult> ExecuteReader<TResult>(this IDbCommand iDbCommand, Func<IDataRecord, TResult> func)
		{
			using var iDataReader = iDbCommand.ExecuteReader();
			try
			{
				return iDataReader.ExecuteReader(func).ToArray();
			}
			finally
			{
				iDataReader.Close();
				iDbCommand.Dispose();
			}
		}

		public static IEnumerable<TResult> ExecuteReader<TResult>(this IDataReader iDataReader, Func<IDataRecord, TResult> func)
		{
			while (iDataReader.Read())
				yield return func(iDataReader);
		}

		public static TimeSpan GetTimeSpan(this IDataRecord dataRecord, int index)
		{
			return (TimeSpan)dataRecord.GetValue(index);
		}
	}
}