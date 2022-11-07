using System;
using System.Data;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public class ConnectionManager<TConnectionDialect> : IConnectionManager, IDisposable where TConnectionDialect : IConnectionDialect, new()
	{
		private readonly object _acesso = new object();
		private readonly IDbConnection _iDbConnection;
		private readonly TConnectionDialect _dialect;
		private IDbTransaction _iDbTransaction = null;
		private int _transactionCount = 0;

		IConnectionDialect IConnectionManager.Dialect => _dialect;
		IDbConnection IConnectionManager.Connection => GetDbConnection();
		IDbTransaction IConnectionManager.Transaction => _iDbTransaction;

		public ConnectionManager(string connectionString)
		{
			_dialect = new TConnectionDialect();
			_iDbConnection = _dialect.CreateConnection(connectionString);
		}

		private IDbConnection GetDbConnection()
		{
			Open();
			return _iDbConnection;
		}

		public void Open()
		{
			if (_iDbConnection.State != ConnectionState.Open)
				_iDbConnection.Open();
		}

		public void Close()
		{
			if (_iDbConnection.State != ConnectionState.Closed)
				_iDbConnection.Close();
		}

		public void Dispose()
		{
			Close();
			_iDbConnection.Dispose();
		}

		IDbTransaction IConnectionManager.BeginTransaction()
		{
			lock (_acesso)
			{
				if (_transactionCount == 0)
					_iDbTransaction = GetDbConnection().BeginTransaction();
				Interlocked.Increment(ref _transactionCount);
			}

			return _iDbTransaction;
		}

		void IConnectionManager.EndTransaction(bool commit)
		{
			lock (_acesso)
			{
				if (commit)
					Commit();
				else
					RollBack();

				CleanUp();
			}
		}

		#region // "Commit / RollBack / Cleanup"
		private void Commit()
		{
			if (_transactionCount == 1)
				_iDbTransaction?.Commit();
			Interlocked.Decrement(ref _transactionCount);
		}

		private void RollBack()
		{
			_iDbTransaction?.Rollback();
			Interlocked.Exchange(ref _transactionCount, 0);
		}

		private void CleanUp()
		{
			if (_transactionCount == 0)
			{
				_iDbTransaction?.Dispose();
				_iDbTransaction = null;
			}
		}
		#endregion // "Commit / RollBack / Cleanup"

		public virtual TClass CreateInstance<TClass>(Action<TClass> setup = null) where TClass : new()
		{
			var instance = new TClass();
			return Configure(instance, setup);
		}

		public virtual TClass CreateInstance<TClass>(Action<TClass> setup = null, params object[] parametros) where TClass : class
		{
			var instance = Activator.CreateInstance(typeof(TClass), parametros) as TClass;
			return Configure(instance, setup);
		}

		protected virtual TClass Configure<TClass>(TClass instance, Action<TClass> setup)
		{
			SetConnectionManager(instance as INeedConnectionManager);

			setup?.Invoke(instance);

			return instance;
		}

		public virtual void SetConnectionManager(INeedConnectionManager needConnectionManager)
		{
			needConnectionManager?.SetConnectionManager(this);
		}
	}
}