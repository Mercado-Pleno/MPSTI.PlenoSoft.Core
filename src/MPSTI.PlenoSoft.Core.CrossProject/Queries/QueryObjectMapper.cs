using MPSTI.PlenoSoft.Core.CrossProject.Domains;
using System;
using System.Collections.Generic;
using System.Data;

namespace MPSTI.PlenoSoft.Core.CrossProject.Queries
{
	public abstract class QueryObjectMapper<TEntidade> : QueryObject<TEntidade> where TEntidade : IEntidade, new()
	{
		public QueryObjectMapper(string tabela, string primaryKey, params string[] campos) : base(tabela, primaryKey, campos) { }

		protected abstract void ConfigurarParametros(Dictionary<string, object> parametros, TEntidade entidade);
		protected override abstract TEntidade DataRecordToEntity(IDataRecord dataRecord);

		protected virtual Dictionary<string, object> ObterParametros(TEntidade entidade)
		{
			var parametros = new Dictionary<string, object> { { PrimaryKey, entidade.Id } };
			ConfigurarParametros(parametros, entidade);
			return parametros;
		}

		protected virtual IEnumerable<TEntidade> ObterPor(TEntidade entidade, string cmdSql)
		{
			var parametros = ObterParametros(entidade);
			return ExecuteReader(cmdSql, parametros);
		}

		public virtual void Incluir(params TEntidade[] entidades)
		{
			foreach (var entidade in entidades)
				Incluir(entidade);
		}

		public virtual void Incluir(TEntidade entidade)
		{
			var parametros = ObterParametros(entidade);
			entidade.Id = ExecuteScalar(SqlInsert, parametros);
		}

		public virtual void Alterar(TEntidade entidade)
		{
			var parametros = ObterParametros(entidade);
			ExecuteScalar(SqlUpdate, parametros);
		}

		[Obsolete("\r\nEi, NÃO use: ExecuteReader(string, IDictionary<string, object>, Func<IDataRecord, TResult>)\r\nOu seja, use: ExecuteReader(string, IDictionary<string, object>)", true)]
		protected new IEnumerable<TResult> ExecuteReader<TResult>(string cmdSQL, IDictionary<string, object> parameters, Func<IDataRecord, TResult> func)
		{
			return base.ExecuteReader(cmdSQL, parameters, func);
		}
	}
}