using System;
using System.Diagnostics;
using System.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
	[DebuggerNonUserCode]
	public abstract class DatabaseTest : AbstractTest, IDisposable
	{
		private readonly TransactionScope _transactionScope;
		protected DatabaseTest(ITestOutputHelper testOutputHelper = null) : base(testOutputHelper) => _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		~DatabaseTest() => Dispose();

		public void Dispose() => RollBack();

		protected void RollBack() => _transactionScope.Dispose();
	}

	[DebuggerNonUserCode]
	public abstract class DatabaseTest<TSingletonTestContext> : DatabaseTest, IClassFixture<TSingletonTestContext> where TSingletonTestContext : class, IDisposable
	{
		protected readonly TSingletonTestContext SingletonTestContext;
		protected DatabaseTest(TSingletonTestContext singletonTestContext, ITestOutputHelper testOutputHelper)
			: base(testOutputHelper) => SingletonTestContext = singletonTestContext;
	}
}