using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using System.Data.SQLite;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections.Dialects
{
	public abstract class SqLiteAbstractDialect<TIDbConnection> : ConnectionDialect<TIDbConnection> where TIDbConnection : IDbConnection, new()
	{
		protected override string PrimaryKey => " Primary Key ";
		protected override string AutoIncrement => " AutoIncrement";
		protected override string GetLastId => "Select Last_Insert_RowId()";
		protected override string NotNull => " Not Null";
		protected override string TypeChar => "Char(1)";
		protected override string TypeBoolean => "SmallInt";
		protected override string TypeInt16 => "SmallInt";
		protected override string TypeInt32 => "Integer";
		protected override string TypeInt64 => "Integer";
		protected override string TypeDecimal12_2 => "Decimal(12, 2)";
		protected override string TypeDate => "Date";
		protected override string TypeDateTime => "DateTime";
		protected override string TypeTime => "Time";
		protected override string TypeTimeStamp => "TimeStamp";
		protected override string TypeGuid => "UniqueIdentifier";
		protected override string TypeString250 => "VarChar(250)";
		protected override string TypeXML => "Text";
		protected override string TypeText => "Text";
		protected override string TypeDefault => "VarChar(4000)";

		public override string GetPrimaryKeyTemplate(string tabela) => $" {TypeInt64} {NotNull} {PrimaryKey} {AutoIncrement}";
	}



	public class SQLiteSystemDialect : SqLiteAbstractDialect<SQLiteConnection> { }



	public class SqliteMicrosoftDialect : SqLiteAbstractDialect<SqliteConnection>
	{
		protected override void OnCreateConnection(string connectionString)
		{
			raw.SetProvider(new SQLite3Provider_winsqlite3());
		}
	}
}