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

        private HttpWebServer? HttpServer { get; set; } 
        HttpWebServer IApplicationBuilder.HttpServer => HttpServer ?? throw new InvalidOperationException("HttpServer is not initialized");
         
        private ApplicationBuilder()
        {
            Configuration = new Configuration();
            HttpServer = new HttpWebServer();  

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
            if(HttpServer == null) throw new InvalidOperationException("HttpServer is not initialized");

            HttpServer.Initialize(GetPrefixes());   

            ControllerResolver.Resolve();

            WebApplication webApplication = new WebApplication(HttpServer);
             
            return webApplication;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in T to the application's service container
        /// </summary>
        /// <typeparam name="T">The type of the service to add</typeparam>
        public void AddSingleton<T>() where T : class, new() 
        {
            var instance = new T();

            ServiceContainer.ConfigureServices(services =>
            { 
                services.AddSingleton(instance);
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

        /// <summary>
        /// Configures the specified options by applying the provided configuration action
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to configure</typeparam>
        /// <param name="configure">The action to apply to the options</param>
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : class, new() => Configuration.Configure(configure);

        /// <summary>
        /// Retrieves the configuration options of the specified type
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to retrieve</typeparam>
        /// <returns>The configured options if found; otherwise, a new instance of the options</returns>
        public TOptions GetConfiguration<TOptions>() where TOptions : class, new() => Configuration.GetConfiguration<TOptions>();

        /// <summary>
        /// Adds scoped services to the application's dependency injection container
        /// </summary>
        /// <param name="action">An action to configure the services</param>
        /// <returns>The updated service collection</returns>
        public void AddScopedServices(Action<ScopedServicesBuilder> action)
        { 
            ServiceContainer.ConfigureServices(services =>
            { 
                using (var scopedServices = new ScopedServicesBuilder(services))
                {
                    action?.Invoke(scopedServices);
                } 
            });
        }  
    }
}
