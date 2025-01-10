using DotNetWebInterface.Application.Core;
using DotNetWebInterface.Validator;
using System.Collections.Specialized; 
using System.Security.Claims;

namespace DotNetWebInterface.Application.Route
{
    /// <summary>
    /// Represents the base class for all routes in the application
    /// </summary>
    public abstract class BaseRoute : IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// This variable is responsible for storing the data passed through authentication
        /// </summary>
        protected IEnumerable<Claim> Claims => Context?.Claims ?? Enumerable.Empty<Claim>();
         
        /// <summary>
        /// Gets the query string parameters for the current request
        /// </summary>
        protected NameValueCollection QueryString => Context?.QueryString ?? new NameValueCollection(); 
         
        /// <summary>
        /// Gets or sets the HTTP context for the current request
        /// </summary>
        protected HttpContext? Context { get; private set; }

        /// <summary>
        /// Assigns the HTTP context to the route
        /// </summary>
        /// <param name="context">The HTTP context for the current request</param>
        internal void SetContext(HttpContext context)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(BaseRoute));
            } 

            Context = context ?? throw new ArgumentNullException(nameof(context)); 
        }
  
        /// <summary>
        /// Writes the specified content to the response with the given status code
        /// </summary>
        /// <param name="content">The content to write to the response</param>
        /// <param name="statusCode">The HTTP status code for the response, Default is 200</param>
        /// <returns>A task that represents the asynchronous write operation</returns>
        protected async Task WriteAsync(string content, int statusCode = 200)
        {
            if (Context != null)
            {
                await Context.WriteAsync(
                    statusCode,
                    content,
                    JsonValidator.IsJson(content) ? "application/json" : "text/plain"
                ); 
            }
            else
            {
                throw new InvalidOperationException("Context is not set.");
            }
        }

        /// <summary>
        /// Disposes the route and releases resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true; 
                Context = null!; 

                GC.SuppressFinalize(this);
            }
        }
         
        ~BaseRoute()
        {
            Dispose();
        }
    }
}
