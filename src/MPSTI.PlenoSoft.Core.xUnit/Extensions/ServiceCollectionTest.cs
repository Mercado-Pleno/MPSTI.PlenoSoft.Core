using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.xUnit.Extensions
{
	public class ServiceCollectionTest
	{
		private readonly Mock<IServiceCollection> _serviceCollectionMock;
		public IServiceCollection ServiceCollection => _serviceCollectionMock.Object;

		public ServiceCollectionTest() => _serviceCollectionMock = new Mock<IServiceCollection>();

		public void ContainsSingletonService<TInstance>() => IsRegistered<TInstance, TInstance>(ServiceLifetime.Singleton);
		public void ContainsSingletonService<TService, TInstance>() => IsRegistered<TService, TInstance>(ServiceLifetime.Singleton);

		public void ContainsScopedService<TInstance>() => IsRegistered<TInstance, TInstance>(ServiceLifetime.Scoped);
		public void ContainsScopedService<TService, TInstance>() => IsRegistered<TService, TInstance>(ServiceLifetime.Scoped);

		public void ContainsTransientService<TInstance>() => IsRegistered<TInstance, TInstance>(ServiceLifetime.Transient);
		public void ContainsTransientService<TService, TInstance>() => IsRegistered<TService, TInstance>(ServiceLifetime.Transient);

		private void IsRegistered<TService, TInstance>(ServiceLifetime lifetime)
		{
			var serviceType = typeof(TService);
			var implementationType = typeof(TInstance);

			_serviceCollectionMock.Verify(
				serviceCollection => serviceCollection.Add(It.Is<ServiceDescriptor>(
					serviceDescriptor => Is(serviceDescriptor, lifetime, serviceType, implementationType))
				)
			);
		}

		private bool Is(ServiceDescriptor serviceDescriptor, ServiceLifetime lifetime, Type serviceType, Type implementationType)
		{
			return serviceDescriptor.Lifetime == lifetime
				&& serviceDescriptor.ServiceType == serviceType
				&& VerifyImplementationType(serviceDescriptor.ImplementationType ?? serviceDescriptor.ImplementationInstance?.GetType() ?? serviceType, implementationType);
			;
		}

		private bool VerifyImplementationType(Type implementationType, Type expectedType)
		{
			return implementationType == expectedType
				|| implementationType.IsSubclassOf(expectedType)
				|| implementationType.GetInterfaces().Any(i => i == expectedType)
			;
		}
	}
}