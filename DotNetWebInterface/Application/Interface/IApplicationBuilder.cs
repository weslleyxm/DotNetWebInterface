using DotNetWebInterface.Server;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface
{
    /// <summary>
    /// Interface for building and configuring a web application
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets the configuration settings for the application
        /// </summary>
        public Configuration Configuration { get; }

        /// <summary>
        /// Gets the HTTP server for the application
        /// </summary>
        public HttpWebServer HttpServer { get; } 

        /// <summary>
        /// Builds the web application
        /// </summary>
        /// <returns>A configured <see cref="WebApplication"/> instance</returns>
        public WebApplication Build();

        /// <summary>
        /// Gets the route prefixes for the application
        /// </summary>
        /// <returns>A string containing the route prefixes</returns>
        public string GetPrefixes();
         
        /// <summary>
        /// Configures the specified options by applying the provided configuration action
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to configure</typeparam>
        /// <param name="configure">The action to apply to the options</param> 
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : class, new();

        /// <summary>
        /// Retrieves the configuration options of the specified type
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to retrieve</typeparam>
        /// <returns>The configured options if found; otherwise, a new instance of the options</returns>
        public TOptions GetConfiguration<TOptions>() where TOptions : class, new();

        /// <summary>
        /// Adds scoped services to the application's dependency injection container
        /// </summary>
        /// <param name="services">An action to configure the services</param>
        /// <returns>The updated service collection</returns>
        public void AddScopedServices(Action<ScopedServicesBuilder> services);
    }
} 