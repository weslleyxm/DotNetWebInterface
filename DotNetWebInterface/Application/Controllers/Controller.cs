using DotNetWebInterface.Server;
using DotNetWebInterface.Validator;
using System.Collections.Specialized;
using System.Security.Claims;
using DotNetWebInterface.Controllers.UserFiles;
using Newtonsoft.Json;

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

        /// <summary>
        /// Collection of user files and provides methods to manage them
        /// </summary>
        protected Files? RequestFiles { get; private set; }

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

            RequestFiles = new Files
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
                    "application/json; charset=utf-8"
                );
            }
            else
            {
                throw new InvalidOperationException("Context is not set");
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


        /// <summary>
        /// Retrieves the value of a specific claim type from the claims collection
        /// </summary>
        /// <param name="claimType">The type of the claim to retrieve</param>
        /// <returns>The value of the claim if found; otherwise, "undefined"</returns>
        protected string GetClaimValue(string claimType)
        {
            var claimValue = Claims.FirstOrDefault(c => c.Type == claimType);
            if (claimValue == null)
            {
                return "undefined";
            }

            return claimValue.Value;
        }

        /// <summary>
        /// Retrieves the value of a claim and deserializes it into the specified type
        /// </summary>
        /// <typeparam name="T">The type to deserialize the claim value into</typeparam>
        /// <param name="claimType">The type of the claim to retrieve</param>
        /// <returns>The deserialized claim value</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the claim type is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when deserialization to the specified type fails</exception>
        public T GetClaimValue<T>(string claimType)
        {
            var claim = Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim == null)
            {
                throw new KeyNotFoundException($"Claim of type '{claimType}' not found");
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(claim.Value) ?? throw new InvalidOperationException($"Failed to deserialize claim value to type '{typeof(T).Name}'");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize claim value to type '{typeof(T).Name}'", ex);
            }
        }

        ~Controller()
        {
            Dispose();
        }
    }
}
