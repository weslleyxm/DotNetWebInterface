using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; 

namespace DotNetWebInterface.Application.Dependency
{
    public static class ServiceContainer
    {
        private static readonly IServiceCollection _services = new ServiceCollection();
        private static IServiceProvider _primaryServiceProvider = _services.BuildServiceProvider();
        private static readonly List<IServiceCollection> _dynamicCollections = new();
         
        public static void ConfigureServices(Action<IServiceCollection> configureServices) 
        {
            var newServices = new ServiceCollection();
            configureServices(newServices);
            _dynamicCollections.Add(newServices);
             
            _primaryServiceProvider = BuildCompositeServiceProvider();
        }
         
        public static T Resolve<T>() where T : notnull
        {
            return _primaryServiceProvider.GetRequiredService<T>();
        }
         
        public static object Resolve(Type type)
        {
            return _primaryServiceProvider.GetRequiredService(type);
        }
         
        private static IServiceProvider BuildCompositeServiceProvider()
        {
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
