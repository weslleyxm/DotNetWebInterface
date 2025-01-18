using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface.Services
{
    /// <summary>
    /// A builder class for registering scoped services in the dependency injection container
    /// </summary>
    public class ScopedServicesBuilder  : IDisposable
    {
        private readonly IServiceCollection _services;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedServicesBuilder"/> class
        /// </summary>
        /// <param name="services">The service collection to add services to</param>
        /// <exception cref="ArgumentNullException">Thrown when the services parameter is null</exception>
        public ScopedServicesBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an implementation
        /// type specified in <typeparamref name="TImplementation"/> to the service collection
        /// </summary>
        /// <typeparam name="TService">The type of the service to add</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use</typeparam>
        public void AddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _services.AddScoped<TService, TImplementation>();
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with a factory specified
        /// in <paramref name="implementationFactory"/> to the service collection
        /// </summary>
        /// <typeparam name="TService">The type of the service to add</typeparam>
        /// <param name="implementationFactory">The factory that creates the service</param>
        public void AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            _services.AddScoped(implementationFactory);
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> to the service collection
        /// </summary>
        /// <typeparam name="TService">The type of the service to add</typeparam>
        public void AddScoped<TService>() where TService : class
        {
            _services.AddScoped<TService>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            { 
                _disposed = true;  
            }
        }

        ~ScopedServicesBuilder()
        {
            Dispose(false); 
        }
    }
}
