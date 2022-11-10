using System;
using System.Diagnostics;
using System.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
	[DebuggerNonUserCode]
	public abstract class DbTest : BaseTest, IDisposable
	{
		private readonly TransactionScope _transactionScope;
		protected DbTest(ITestOutputHelper testOutputHelper = null) : base(testOutputHelper) => _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		~DbTest() => Dispose();

		public void Dispose() => RollBack();

		protected void RollBack() => _transactionScope.Dispose();
	}

	[DebuggerNonUserCode]
	public abstract class DbTest<TSingletonTestContext> : DbTest, IClassFixture<TSingletonTestContext> where TSingletonTestContext : class, IDisposable
	{
		protected readonly TSingletonTestContext SingletonTestContext;
		protected DbTest(TSingletonTestContext singletonTestContext, ITestOutputHelper testOutputHelper)
			: base(testOutputHelper) => SingletonTestContext = singletonTestContext;
	}
}