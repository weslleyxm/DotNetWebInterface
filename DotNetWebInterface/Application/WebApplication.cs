using DotNetWebInterface.Controllers.Role;
using DotNetWebInterface.Cors;
using DotNetWebInterface.Middleware;
using DotNetWebInterface.Server;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface
{
    /// <summary>
    /// Represents a web application that can be configured and run
    /// </summary>
    public class WebApplication : IWebApplication
    {
        private readonly HttpServer httpServer;
        private CorsPolicyBuilder? corsPolicyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplication"/> class
        /// </summary>
        /// <param name="httpServer">The HTTP server to be used by the application</param>
        public WebApplication(HttpServer httpServer)
        {
            this.httpServer = httpServer;
        }

        /// <summary>
        /// Runs the web application asynchronously
        /// </summary>
        public async Task Run()
        {
            SetDefaultMiddlewares();

            Console.ResetColor(); 

            await httpServer.RunAsync();
        } 

        /// <summary>
        /// Configures CORS policy for the web application
        /// </summary>
        /// <param name="action">An action to configure the CORS policy</param>
        public void UseCors(Action<CorsPolicyBuilder> action)
        {
            corsPolicyBuilder = new CorsPolicyBuilder();
            httpServer.AddCorsPolicy(corsPolicyBuilder);

            action?.Invoke(corsPolicyBuilder);
        }

        /// <summary>
        /// Adds middleware to the web application
        /// </summary>
        /// <typeparam name="T">The type of middleware to add</typeparam>
        void IWebApplication.UseMiddleware<T>() where T : class
        {
            httpServer.AddMiddleware<T>();
        }
          
        /// <summary>
        /// Sets the route prefix for the web application
        /// </summary>
        /// <param name="prefix">The route prefix to set</param>
        void IWebApplication.SetRoutePrefix(string prefix)
        {
            if (httpServer != null)
            {
                httpServer.SetRoutePrefix(prefix);
            }
            else
            {
                throw new InvalidOperationException("HttpServer is not initialized.");
            }
        }

        /// <summary>
        /// Sets the default middlewares for the web application
        /// </summary>
        internal void SetDefaultMiddlewares()
        {
            ServiceContainer.ConfigureServices(services =>
            {
                services.AddSingleton<IRoleProvider>(provider => new JwtRoleExtractor());  
            }); 

            httpServer.AddMiddleware<RoleMiddleware>();
            httpServer.AddMiddleware<FileMiddleware>(); 
        }
    }
} 