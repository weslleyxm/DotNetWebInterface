using DotNetWebInterface.Config;
using DotNetWebInterface.Middleware;
using DotNetWebInterface.Multipart.Middleware;

namespace DotNetWebInterface.Extensions
{
    /// <summary>
    /// Extension methods for configuring the web application
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
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
        /// Adds role-based authorization middleware to the application pipeline
        /// </summary>
        /// <param name="app">The application instance</param>
        /// <returns>The updated application instance</returns>
        public static IWebApplication UseRoleAuthorization(this IWebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<RoleMiddleware>();
            return app;
        }

        /// <summary>
        /// Adds file handling support middleware to the application pipeline
        /// </summary>
        /// <param name="app">The application instance</param>
        /// <returns>The updated application instance</returns>
        public static IWebApplication UseMultipartSupport(this IWebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            app.UseMiddleware<MultipartMiddleware>();
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

        /// <summary>
        /// Configures the default settings for the web application
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="configureOptions">An action to configure the application options</param>
        public static void ConfigureAppDefaults(this IWebApplication app, Action<AppConfigurationOptions> configureOptions)
        {
            var options = new AppConfigurationOptions();
            configureOptions(options);

            if (options.EnableCors)
            {
                app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            }

            if (options.EnableFileProcessing)
            {
                app.UseMiddleware<MultipartMiddleware>();
            }

            if (!string.IsNullOrEmpty(options.ApiPrefix))
            {
                app.SetPrefix(options.ApiPrefix);
            }

            if (options.SQLInjectionCountermeasures)
            {
                app.UseSQLInjectionCountermeasures();
            }
        }
    }
}
