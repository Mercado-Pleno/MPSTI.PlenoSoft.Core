using System.Data;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Queries
{
	public abstract class QueryObject : Query, IQueryObject
	{
		protected readonly string Tabela;
		protected readonly string PrimaryKey;
		protected readonly string[] Campos;
		protected readonly string[] CamposSelect;
		protected readonly string[] CamposInsertUpdate;

		public abstract string SqlCreateTable { get; }
		public virtual string SqlDropTable => $"Drop Table {Tabela}; ";
		public virtual string SqlSelect => $"Select {string.Join(", ", CamposSelect)} From {Tabela} ";
		public virtual string SqlInsert => $"Insert Into {Tabela} ({string.Join(", ", CamposInsertUpdate)}) Values ({string.Join(", ", CamposInsertUpdate.Select(c => "@" + c))}); {ConnectionManager.Dialect.GetCmdSqlLastId()}; ";
		public virtual string SqlUpdate => $"Update {Tabela} Set {string.Join(", ", CamposInsertUpdate.Select(c => $"{c} = @{c}"))} Where ({PrimaryKey} = @{PrimaryKey}) ";
		public virtual string SqlDeleteAll => $"Delete From {Tabela} ";
		public virtual string SqlDelete => $"{SqlDeleteAll} Where ({PrimaryKey} = @{PrimaryKey}) ";

		public QueryObject(string tabela, string primaryKey, params string[] campos)
		{
			Tabela = tabela;
			PrimaryKey = primaryKey;
			CamposInsertUpdate = campos;
			CamposSelect = new[] { primaryKey }.Union(campos).ToArray();
			Campos = CamposSelect.Select(c => c.ToUpper()).ToArray();
		}

		protected int IndexOf(string campo) => IndexOf(Campos, campo.ToUpper());

		public virtual void CreateTable()
		{
			ExecuteNonQuery(SqlCreateTable);
		}

		public virtual void DropTable()
		{
			ExecuteNonQuery(SqlDropTable);
		}
	}
}