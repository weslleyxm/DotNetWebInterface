using DotNetWebInterface.Application.Dependency;
using DotNetWebInterface.Application.Middleware;
using Microsoft.Extensions.DependencyInjection; 

namespace DotNetWebInterface.Application.Services.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds authentication services to the application builder
        /// </summary>
        /// <param name="builder">The application builder</param>
        public static void AddAuthenticationService(this IApplicationBuilder builder)
        {
            ServiceContainer.ConfigureServices(services =>
            {
                services.AddSingleton<AuthenticationService>();
                services.AddTransient<BearerAuthorizationMiddleware>();
            });
        }
    }
}
