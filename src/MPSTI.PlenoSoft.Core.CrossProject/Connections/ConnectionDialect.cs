using System;
using System.Data;
using System.Text;
using System.Xml;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public abstract class ConnectionDialect<TIDbConnection> : IConnectionDialect where TIDbConnection : IDbConnection, new()
	{
		protected abstract string PrimaryKey { get; }
		protected abstract string AutoIncrement { get; }
		protected abstract string GetLastId { get; }
		protected abstract string NotNull { get; }
		protected abstract string TypeChar { get; }
		protected abstract string TypeBoolean { get; }
		protected abstract string TypeInt16 { get; }
		protected abstract string TypeInt32 { get; }
		protected abstract string TypeInt64 { get; }
		protected abstract string TypeDecimal12_2 { get; }
		protected abstract string TypeDate { get; }
		protected abstract string TypeDateTime { get; }
		protected abstract string TypeTime { get; }
		protected abstract string TypeTimeStamp { get; }
		protected abstract string TypeGuid { get; }
		protected abstract string TypeString250 { get; }
		protected abstract string TypeXML { get; }
		protected abstract string TypeText { get; }
		protected abstract string TypeDefault { get; }

		public abstract string GetPrimaryKeyTemplate(string tabela);
		protected virtual string DoConvertType(Type type) => TypeDefault;
		protected virtual void OnCreateConnection(string connectionString) { }

		public string GetCmdSqlLastId() => GetLastId;

		public IDbConnection CreateConnection(string connectionString)
		{
			OnCreateConnection(connectionString);
			return new TIDbConnection { ConnectionString = connectionString };
		}

		public string ConvertType(Type propType)
		{
			var rootType = Nullable.GetUnderlyingType(propType);
			var isNull = rootType == null ? NotNull : string.Empty;
			var type = rootType ?? propType;

			if (type == typeof(string))
				return TypeString250 + isNull;
			else if (type == typeof(char))
				return TypeChar + isNull;
			else if (type == typeof(bool))
				return TypeBoolean + isNull;
			else if (type.IsEnum)
				return TypeInt16 + isNull;
			else if (type == typeof(short))
				return TypeInt16 + isNull;
			else if (type == typeof(int))
				return TypeInt32 + isNull;
			else if (type == typeof(long))
				return TypeInt64 + isNull;
			else if (type == typeof(decimal))
				return TypeDecimal12_2 + isNull;
			else if (type == typeof(float))
				return TypeDecimal12_2 + isNull;
			else if (type == typeof(double))
				return TypeDecimal12_2 + isNull;
			else if (type == typeof(DateTime))
				return TypeDateTime + isNull;
			else if (type == typeof(TimeSpan))
				return TypeTime + isNull;
			else if (type == typeof(Guid))
				return TypeGuid + isNull;
			else if (type == typeof(StringBuilder))
				return TypeText + isNull;
			else if (type == typeof(XmlNode))
				return TypeXML + isNull;

			return DoConvertType(type);
		}
	}
}