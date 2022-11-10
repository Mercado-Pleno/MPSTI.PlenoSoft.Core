using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Extensions.Interfaces;
using MPSTI.PlenoSoft.Core.Extensions.Providers;
using MPSTI.PlenoSoft.Core.xUnit.Factories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
	[DebuggerNonUserCode]
	public abstract class AbstractTest
	{
		protected readonly ITestOutputHelper _testOutputHelper;
		protected readonly IConfiguration configuration = ConfigurationFactory.Configuration;
		protected readonly IFixture Fixture = new Fixture();
		protected IServiceCollection _serviceCollection;
		protected IServiceProvider _serviceProvider;
		protected AbstractTest(ITestOutputHelper testOutputHelper = null) => _testOutputHelper = testOutputHelper;
		protected void Trace(string message) => _testOutputHelper?.WriteLine(message);


		protected virtual async Task<TService> GetServiceAsync<TService>() => await Task.FromResult(GetService<TService>());

		protected virtual TService GetService<TService>() => ServiceProvider.GetRequiredService<TService>();

		protected virtual IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider();

		protected virtual IServiceCollection ServiceCollection => _serviceCollection ??= CreateDefaultServiceCollection();

		protected virtual IServiceCollection CreateDefaultServiceCollection()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton(sp => configuration);
			serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
			serviceCollection.AddSingleton<IFormatProviders, FormatProviders>();
			serviceCollection.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("Test"));
			ConfigureServices(serviceCollection);
			return serviceCollection;
		}

		protected virtual void ConfigureServices(IServiceCollection services) { }
	}

	[DebuggerNonUserCode]
	public abstract class AbstractTest<TSingletonTestContext> : AbstractTest, IClassFixture<TSingletonTestContext> where TSingletonTestContext : class, IDisposable
	{
		protected readonly TSingletonTestContext SingletonTestContext;
		protected AbstractTest(TSingletonTestContext singletonTestContext, ITestOutputHelper testOutputHelper = null)
			: base(testOutputHelper) => SingletonTestContext = singletonTestContext;
	}
}