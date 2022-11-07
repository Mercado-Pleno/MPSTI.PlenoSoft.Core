using MPSTI.PlenoSoft.Core.CrossProject.Domains;
using MPSTI.PlenoSoft.Core.CrossProject.Utils;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Queries
{
	public class QueryObject<TEntidade> : QueryObject where TEntidade : IEntidade, new()
	{
		public QueryObject(string tabela, string primaryKey, params string[] campos) : base(tabela, primaryKey, campos) { }

		protected virtual void ConfigurarLazyLoad(TEntidade entidade) { }

		protected virtual TEntidade DataRecordToEntity(IDataRecord dataRecord)
		{
			return ReflectionUtil.Fill<TEntidade>(dataRecord);
		}

		public virtual void Excluir(TEntidade entidade)
		{
			var parametros = new Dictionary<string, object> { { PrimaryKey, entidade.Id } };
			ExecuteScalar(SqlDelete, parametros);
		}

		protected IEnumerable<TEntidade> ExecuteReader(string cmdSQL, IDictionary<string, object> parameters)
		{
			return base.ExecuteReader(cmdSQL, parameters, ConvertDataRecordToEntity);
		}

		public virtual IEnumerable<TEntidade> ObterTodos()
		{
			return ExecuteReader(SqlSelect, null);
		}

		public virtual IEnumerable<TEntidade> ObterPorId(long id)
		{
			var cmdSql = SqlSelect + $" Where ({PrimaryKey} = @{PrimaryKey})";
			var parametros = new Dictionary<string, object> { { PrimaryKey, id } };
			return ExecuteReader(cmdSql, parametros);
		}

		protected virtual TEntidade ConvertDataRecordToEntity(IDataRecord dataRecord)
		{
			var entidade = DataRecordToEntity(dataRecord);
			ConfigurarLazyLoad(entidade);
			return entidade;
		}

		protected virtual string ObterCmdSqlSelectPor(string where = "") => SqlSelect + where;

		public override string SqlCreateTable
		{
			get
			{
				var cache = ReflectionUtil.GetTypes<TEntidade>();
				var campos = CamposInsertUpdate.Select(c => $"{c} {ConnectionManager.Dialect.ConvertType(cache[c.ToUpper()])}");
				var primaryKeyTemplate = ConnectionManager.Dialect.GetPrimaryKeyTemplate(Tabela);
				var cmdSql = $@"Create Table {Tabela} (
	{PrimaryKey} {primaryKeyTemplate},
	{string.Join(",\r\n\t", campos)}
);";

				return cmdSql;
			}
		}
	}
}