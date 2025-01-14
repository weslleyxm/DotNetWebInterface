using DotNetWebInterface.Middleware;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds authentication services to the application builder.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="provideSecret">A function that provides the secret key.</param>
        public static void AddAuthenticationService(this IApplicationBuilder builder, Func<string> provideSecret)
        {
            var secretKey = provideSecret();

            ServiceContainer.ConfigureServices(services =>
            {
                services.AddSingleton<AuthenticationService>(provider => new AuthenticationService(secretKey)); 
            });
        }
    }
}
