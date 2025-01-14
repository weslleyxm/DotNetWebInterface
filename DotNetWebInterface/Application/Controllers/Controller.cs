using DotNetWebInterface.Server;
using DotNetWebInterface.Validator;
using System.Collections.Specialized;
using System.Security.Claims;
using DotNetWebInterface.Controllers.UserFiles;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Represents the base class for all controllers in the application
    /// </summary>
    public abstract class Controller : IDisposable
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

        protected Files? Files { get; private set; } 

        /// <summary>
        /// Assigns the HTTP context to the controller
        /// </summary>
        /// <param name="context">The HTTP context for the current request</param>
        internal void SetContext(HttpContext context)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Controller));
            }

            Context = context ?? throw new ArgumentNullException(nameof(context));

            Files = new Files
            {
                Context = context
            }; 
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
                    JsonValidator.IsJson(content) ? "application/json; charset=utf-8" : "text/plain; charset=utf-8"
                );
            }
            else
            {
                throw new InvalidOperationException("Context is not set.");
            }
        }

        /// <summary>
        /// Disposes the controller and releases resources
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

        ~Controller()
        {
            Dispose();
        }
    }
}
