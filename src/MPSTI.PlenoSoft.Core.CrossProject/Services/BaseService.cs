using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.CrossProject.Services
{
	public class BaseService : INeedConnectionManager
	{
		protected IConnectionManager ConnectionManager { get; set; }
		IConnectionManager INeedConnectionManager.ConnectionManager => ConnectionManager;
		protected IDbConnection Connection => ConnectionManager.Connection;
		protected IDbTransaction Transaction => ConnectionManager.Transaction;

		protected void Transactional(Action<IDbTransaction> acao)
		{
			TransactionalImpl(dbTransaction => { acao.Invoke(dbTransaction); return true; });
		}

		protected TResult Transactional<TResult>(Func<IDbTransaction, TResult> acao)
		{
			return TransactionalImpl(acao);
		}

		private TResult TransactionalImpl<TResult>(Func<IDbTransaction, TResult> acao)
		{
			var commit = true;
			try
			{
				var dbTransaction = ConnectionManager?.BeginTransaction();
				return acao.Invoke(dbTransaction);
			}
			catch
			{
				commit = false;
				throw;
			}
			finally
			{
				ConnectionManager?.EndTransaction(commit);
			}
		}

		public void SetConnectionManager(IConnectionManager connectionManager)
		{
			ConnectionManager = connectionManager;
		}
	}
}