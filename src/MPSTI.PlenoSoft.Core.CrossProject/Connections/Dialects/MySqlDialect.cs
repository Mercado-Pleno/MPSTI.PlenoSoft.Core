using MySql.Data.MySqlClient;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections.Dialects
{
	public class MySqlDialect : ConnectionDialect<MySqlConnection>
	{
		protected override string PrimaryKey => " Primary Key ";
		protected override string AutoIncrement => " Auto_Increment";
		protected override string GetLastId => "Select Last_Insert_Id()";
		protected override string NotNull => " Not Null";
		protected override string TypeChar => "Char(1)";
		protected override string TypeBoolean => "Boolean";
		protected override string TypeInt16 => "SmallInt";
		protected override string TypeInt32 => "Int";
		protected override string TypeInt64 => "BigInt";
		protected override string TypeDecimal12_2 => "Decimal(12, 2)";
		protected override string TypeDate => "Date";
		protected override string TypeDateTime => "DateTime";
		protected override string TypeTime => "Time";
		protected override string TypeTimeStamp => "TimeStamp";
		protected override string TypeGuid => "Char(64)";
		protected override string TypeString250 => "VarChar(250)";
		protected override string TypeXML => "Text";
		protected override string TypeText => "Text";
		protected override string TypeDefault => "VarChar(4000)";

		public override string GetPrimaryKeyTemplate(string tabela) => $"{TypeInt64} {NotNull} {AutoIncrement} {PrimaryKey}";
	}
}