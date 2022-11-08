using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace MPSTI.PlenoSoft.Core.xUnit.Extensions
{
	public static class ServiceCollectionMockExtension
	{
		public static TService CreateMock<TService>(this IServiceProvider serviceProvider, Action<Mock<TService>> mockSetup = null) where TService : class
		{
			var mockService = new Mock<TService>(serviceProvider);
			mockSetup?.Invoke(mockService);
			return mockService.Object;
		}

		public static IServiceCollection AddSingletonMock<TService>(this IServiceCollection serviceCollection, Action<Mock<TService>> mockSetup = null) where TService : class
		{
			return serviceCollection.AddSingleton(serviceProvider => serviceProvider.CreateMock(mockSetup));
		}

		public static IServiceCollection AddScopedMock<TService>(this IServiceCollection serviceCollection, Action<Mock<TService>> mockSetup = null) where TService : class
		{
			return serviceCollection.AddScoped(serviceProvider => serviceProvider.CreateMock(mockSetup));
		}

		public static IServiceCollection AddTransientMock<TService>(this IServiceCollection serviceCollection, Action<Mock<TService>> mockSetup = null) where TService : class
		{
			return serviceCollection.AddTransient(serviceProvider => serviceProvider.CreateMock(mockSetup));
		}
	}
}