using MPSTI.PlenoSoft.Core.CrossProject.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections.Data
{
	public static class DbUtilExtension
	{
		public static IEnumerable<string> Parse<T>(this IEnumerable<T> listValues, DbType? dbType)
		{
			var stringValues = dbType.IsString(listValues)
				? listValues.Select(v => v.ParseString(dbType))
				: listValues.Select(v => v?.ToString() ?? "Null");
			return stringValues;
		}

		public static string ParseString<T>(this T self, DbType? dbType)
		{
			if (self == null)
				return "Null";
			else if (self is DateTime dateTime)
			{
				if (dbType.HasValue)
				{
					if (dbType == DbType.Date)
						return $"'{dateTime:yyyy-MM-dd}'";
					else if (dbType == DbType.DateTime)
						return $"'{dateTime:yyyy-MM-dd HH:mm:ss.fff}'";
					else if (dbType == DbType.DateTime2)
						return $"'{dateTime:yyyy-MM-dd HH:mm:ss.fffffff}'";
					else if (dbType == DbType.DateTimeOffset)
						return $"'{dateTime:yyyy-MM-dd HH:mm:ss.fffffff}'";
					else if (dbType == DbType.Time)
						return $"'{dateTime:HH:mm:ss.fff}'";
				}
				return dateTime == dateTime.Date ? $"'{dateTime:yyyy-MM-dd}'" : $"'{dateTime:yyyy-MM-dd HH:mm:ss.fff}'";
			}
			else if (self is TimeSpan timeSpan)
			{
				return $"'{timeSpan:hh\\:mm\\:ss\\.fff}'";
			}
			else if (self is Guid guid)
			{
				return $"'{guid}'";
			}

			return $"'{self}'";
		}

		public static bool IsString<T>(this DbType? dbType, IEnumerable<T> listValues)
		{
			if (dbType.HasValue)
			{
				return dbType.Value.In(DbType.String, DbType.AnsiString, DbType.AnsiStringFixedLength, DbType.Guid, DbType.Date, DbType.DateTime, DbType.DateTime2, DbType.Time, DbType.DateTimeOffset, DbType.Xml);
			}
			else if (listValues?.Any() ?? false)
			{
				var type = listValues.FirstOrDefault(o => o != null)?.GetType();
				var primitiveType = Nullable.GetUnderlyingType(type) ?? type;
				return primitiveType?.In(typeof(string), typeof(StringBuilder), typeof(char), typeof(Guid), typeof(DateTime)) ?? false;
			}
			return false;
		}

		public static string Replace(string commandText, string parameterName, string parameterValue)
		{
			var pattern = $@"({parameterName})[^A-Za-z0-9_]+?";
			var regex = new Regex(pattern, RegexOptions.IgnoreCase);
			var matches = regex.Matches(commandText + " ").OfType<Match>().Where(m => m.Groups.Count > 0).Reverse().ToArray();

			var output = commandText;
			foreach (var match in matches)
			{
				var group = match.Groups[1];
				output = output.Remove(group.Index, group.Length).Insert(group.Index, parameterValue);
			}

			return output;
		}
	}
}