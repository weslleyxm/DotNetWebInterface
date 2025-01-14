using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface
{
    public static class Factory
    {
        public static T Create<T>(Type type, params object[] args)
        {
            var serviceProvider = ServiceContainer.Resolve<IServiceProvider>();
            return (T)ActivatorUtilities.CreateInstance(serviceProvider, type, args); ;
        }
    }
}
