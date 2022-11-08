using System;
using System.Transactions;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
	public abstract class DbTest : AbstractTest, IDisposable
    {
        private readonly TransactionScope _transactionScope;

        public DbTest() => _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        ~DbTest() => Dispose();

        public void Dispose() => RollBack();

        protected void RollBack() => _transactionScope.Dispose();
    }
}