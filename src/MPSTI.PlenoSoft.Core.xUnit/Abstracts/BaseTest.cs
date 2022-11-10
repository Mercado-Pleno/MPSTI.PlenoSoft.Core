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
	public abstract class BaseTest
	{
		private readonly ITestOutputHelper _testOutputHelper;
		private readonly IConfiguration _configuration;

		private IFixture _fixture;
		protected virtual IFixture Fixture => _fixture ??= new Fixture();

		private IServiceProvider _serviceProvider;
		protected virtual IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider();

		private IServiceCollection _serviceCollection;
		protected virtual IServiceCollection ServiceCollection => _serviceCollection ??= CreateDefaultServiceCollection();

		protected BaseTest(ITestOutputHelper testOutputHelper = null)
		{
			_testOutputHelper = testOutputHelper;
			_configuration = ConfigurationFactory.Configuration;
		}

		protected virtual async Task<TService> GetServiceAsync<TService>() => await Task.FromResult(GetService<TService>());

		protected virtual TService GetService<TService>() => ServiceProvider.GetRequiredService<TService>();

		protected virtual IServiceCollection CreateDefaultServiceCollection()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton(sp => _configuration);
			serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
			serviceCollection.AddSingleton<IFormatProviders, FormatProviders>();
			serviceCollection.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("Test"));
			ConfigureServices(serviceCollection, _configuration);
			return serviceCollection;
		}

		protected virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

		protected virtual void Trace(string message) => _testOutputHelper?.WriteLine(message);
	}

	[DebuggerNonUserCode]
	public abstract class BaseTest<TSingletonTestContext> : BaseTest, IClassFixture<TSingletonTestContext> where TSingletonTestContext : class, IDisposable
	{
		protected readonly TSingletonTestContext SingletonTestContext;
		protected BaseTest(TSingletonTestContext singletonTestContext, ITestOutputHelper testOutputHelper = null)
			: base(testOutputHelper) => SingletonTestContext = singletonTestContext;
	}
}