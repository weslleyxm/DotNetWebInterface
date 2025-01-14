using DotNetWebInterface.Config;
using DotNetWebInterface.Controllers;
using DotNetWebInterface.Server;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface
{
    /// <summary>
    /// Provides a builder for creating and configuring a web application
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        public Configuration Configuration { get; private set; }
        private HttpServer? HttpServer { get; set; }

        private ApplicationBuilder()
        {
            Configuration = new Configuration();

            ServiceContainer.ConfigureServices(services =>
            {
                services.AddSingleton<IApplicationBuilder>(provider => this);
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApplicationBuilder"/> class
        /// </summary>
        /// <returns>A new instance of <see cref="ApplicationBuilder"/></returns>
        public static ApplicationBuilder Create()
        {
            return new ApplicationBuilder();
        }

        /// <summary>
        /// Builds the web application by configuring the HTTP server and resolving controllers
        /// </summary>
        /// <returns>A configured instance of <see cref="WebApplication"/></returns>
        public WebApplication Build()
        {
            HttpServer = new HttpServer(GetPrefixes());

            ControllerResolver.Resolve();

            WebApplication webApplication = new WebApplication(HttpServer);
            return webApplication;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in T to the application's service container
        /// </summary>
        /// <typeparam name="T">The type of the service to add</typeparam>
        public void AddSingleton<T>() where T : class
        {
            ServiceContainer.ConfigureServices(services =>
            {
                services.AddSingleton<T>();
            });
        }

        /// <summary>
        /// Adds a scoped service of the type specified in IService with an implementation type specified in TImplementation to the application's service container
        /// </summary>
        /// <typeparam name="IService">The interface type of the service to add</typeparam>
        /// <typeparam name="TImplementation">The implementation type of the service to add</typeparam>
        public void AddScoped<IService, TImplementation>()
        where IService : class
        where TImplementation : class, IService
        {
            ServiceContainer.ConfigureServices(services =>
            {
                services.AddScoped<IService, TImplementation>();
            });
        }

        /// <summary>
        /// Gets the URL prefixes for the HTTP server
        /// </summary>
        /// <returns>A string containing the URL prefixes</returns>
        public string GetPrefixes()
        {
            string prefix = LaunchSettings.ApplicationUrl ?? "http://localhost";
            return prefix;
        }
         
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : class, new() => Configuration.Configure(configure);
         
        public TOptions GetConfiguration<TOptions>() where TOptions : class, new() => Configuration.GetConfiguration<TOptions>();
    }
}
