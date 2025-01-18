using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotNetWebInterface.Services
{
    /// <summary>
    /// Static class that manages the service container and allows dynamic configuration of services
    /// </summary>
    public static class ServiceContainer
    {
        private static readonly IServiceCollection _services = new ServiceCollection();
        private static IServiceProvider _primaryServiceProvider = _services.BuildServiceProvider();
        private static readonly List<IServiceCollection> _dynamicCollections = new();

        /// <summary>
        /// Configures services by adding them to a new service collection and rebuilding the composite service provider
        /// </summary>
        /// <param name="configureServices">An action to configure the services</param>
        public static void ConfigureServices(Action<IServiceCollection> configureServices)
        {
            var newServices = new ServiceCollection();
            configureServices(newServices);
            _dynamicCollections.Add(newServices);

            _primaryServiceProvider = BuildCompositeServiceProvider();
        }

        /// <summary>
        /// Resolves a service of type T from the service provider
        /// </summary>
        /// <typeparam name="T">The type of service to resolve</typeparam>
        /// <returns>The resolved service of type T</returns>
        public static T Resolve<T>() where T : notnull
        {
            var scope = _primaryServiceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Resolves a service of the specified type and executes the provided action safely within a scoped context.
        /// </summary>
        /// <typeparam name="T">The type of the service to resolve.</typeparam>
        /// <param name="action">The action to execute using the resolved service.</param>
        public static async Task ResolveScopedServiceAsync<T>(Func<T, Task> action) where T : notnull
        { 
            using var scope = _primaryServiceProvider.CreateScope();
             
            T service = scope.ServiceProvider.GetRequiredService<T>()!;
             
            if (action != null)
            {
                await action(service).ConfigureAwait(false);
            }
        }
         
        /// <summary>
        /// Resolves a service of the specified type from the service provider
        /// </summary>
        /// <param name="type">The type of service to resolve</param>
        /// <returns>The resolved service</returns>
        public static object Resolve(Type type)
        {
            return _primaryServiceProvider.GetRequiredService(type);
        }

        /// <summary>
        /// Builds a composite service provider from the static and dynamic service collections
        /// </summary>
        /// <returns>The composite service provider</returns>
        private static IServiceProvider BuildCompositeServiceProvider()
        {
            if (_primaryServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            var combinedServices = new ServiceCollection();
            foreach (var collection in _dynamicCollections)
            {
                foreach (var descriptor in collection)
                {
                    combinedServices.Add(descriptor);
                }
            }

            foreach (var descriptor in _services)
            {
                combinedServices.Add(descriptor);
            }

            return combinedServices.BuildServiceProvider();
        }
    }
}
