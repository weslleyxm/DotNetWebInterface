using DotNetWebInterface.Authentication;
using DotNetWebInterface.Middleware;

namespace DotNetWebInterface.Extensions
{
    /// <summary>
    /// Extension methods for configuring the web application
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// Adds Bearer Authentication middleware to the application
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <returns>The web application instance with Bearer Authentication middleware added</returns>
        public static IWebApplication UseBearerAuthentication(this IWebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<BearerAuthorizationMiddleware>();
            return app;
        }

        /// <summary>
        /// Adds SQL Injection Countermeasures middleware to the application
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <returns>The web application instance with SQL Injection Countermeasures middleware added</returns>
        public static IWebApplication UseSQLInjectionCountermeasures(this IWebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<SqlInjectionMiddleware>();
            return app;
        }

        /// <summary>
        /// Sets a route prefix for the application
        /// </summary>
        /// <param name="app">The web application instance</param>
        /// <param name="prefix">The route prefix to set</param>
        /// <returns>The web application instance with the route prefix set</returns>
        public static IWebApplication SetPrefix(this IWebApplication app, string prefix)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.SetRoutePrefix(prefix);
            return app;
        }
    }
}
