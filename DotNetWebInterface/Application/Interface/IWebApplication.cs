using DotNetWebInterface.Config;
using DotNetWebInterface.Cors;
using System;

namespace DotNetWebInterface
{
    /// <summary>
    /// Represents a web application that can be configured and run
    /// </summary>
    public interface IWebApplication
    {
        /// <summary>
        /// Runs the web application asynchronously
        /// </summary>
        public Task Run();

        /// <summary>
        /// Adds middleware to the web application
        /// </summary>
        /// <typeparam name="T">The type of middleware to add</typeparam>
        public void UseMiddleware<T>() where T : class;

        /// <summary>
        /// Configures CORS policy for the web application
        /// </summary>
        /// <param name="action">An action to configure the CORS policy</param>
        public void UseCors(Action<CorsPolicyBuilder> action);

        /// <summary>
        /// Sets the route prefix for the web application
        /// </summary>
        /// <param name="prefix">The route prefix to set</param>
        void SetRoutePrefix(string prefix);   
    }
}
