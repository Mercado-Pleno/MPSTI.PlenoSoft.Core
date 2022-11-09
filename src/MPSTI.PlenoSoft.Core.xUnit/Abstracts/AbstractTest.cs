using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Extensions.Interfaces;
using MPSTI.PlenoSoft.Core.Extensions.Providers;
using MPSTI.PlenoSoft.Core.xUnit.Factories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace MPSTI.PlenoSoft.Core.xUnit.Abstracts
{
    [DebuggerNonUserCode]
    public abstract class AbstractTest
	{
        private readonly ITestOutputHelper _testOutputHelper;
        protected AbstractTest() { }
        protected AbstractTest(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;
        public void WriteLine(string message) => _testOutputHelper?.WriteLine(message);


        private IServiceProvider _serviceProvider;
        protected IConfiguration configuration => ConfigurationFactory.Configuration;

        protected virtual void ConfigureServices(IServiceCollection services) { }

        protected async Task<TService> GetServiceAsync<TService>() => await Task.FromResult(GetService<TService>());

        protected TService GetService<TService>() => GetDefaultServiceProvider().GetRequiredService<TService>();

        private IServiceProvider GetDefaultServiceProvider() => _serviceProvider ??= GetServiceProvider();

        private IServiceProvider GetServiceProvider() => GetServiceCollection().BuildServiceProvider();

        private IServiceCollection GetServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(sp => configuration);
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton<IFormatProviders, FormatProviders>();
            serviceCollection.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("Test"));
            ConfigureServices(serviceCollection);
            return serviceCollection;
        }
    }
}