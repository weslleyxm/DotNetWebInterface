using DotNetWebInterface.Application.Core; 

namespace DotNetWebInterface.Application.Middleware
{
    /// <summary>
    /// Represents a pipeline for handling middleware in the application
    /// </summary>
    public class MiddlewarePipeline
    {
        /// <summary>
        /// List of middleware functions to be executed
        /// </summary>
        private readonly List<Func<HttpContext, Func<Task>, Task>> _middlewares = new();

        /// <summary>
        /// Function to execute the final route
        /// </summary>
        private Func<HttpContext, Task> _routeExecution = null!;

        /// <summary>
        /// Gets the count of middleware functions in the pipeline
        /// </summary>
        public int Count => _middlewares.Count;

        /// <summary>
        /// Adds a middleware function to the pipeline
        /// </summary>
        /// <param name="middleware">The middleware function to add</param>
        public void AddMiddleware(Func<HttpContext, Func<Task>, Task> middleware)
        {
            _middlewares.Add(middleware);
        }

        /// <summary>
        /// Sets the function to execute the final route
        /// </summary>
        /// <param name="routeExecution">The route execution function</param>
        public void SetRouteExecution(Func<HttpContext, Task> routeExecution)
        {
            _routeExecution = routeExecution;
        }

        /// <summary>
        /// Executes the middleware pipeline
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Execute(HttpContext context)
        {
            var index = -1;

            /// <summary>
            /// Executes the next middleware in the pipeline
            /// </summary>
            /// <returns>A task representing the asynchronous operation</returns>
            async Task Next()
            {
                index++;
                if (index < _middlewares.Count)
                {
                    await _middlewares[index](context, Next);
                }
                else
                {
                    await _routeExecution(context);
                }
            }

            await Next();
        }
    }
}
 

