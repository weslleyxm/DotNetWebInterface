using System.Net;

namespace DotNetWebInterface.Cors
{
    /// <summary>
    /// A builder class for configuring CORS policies
    /// </summary>
    public class CorsPolicyBuilder
    {
        private readonly List<string> _allowedOrigins = new();
        private readonly List<string> _allowedHeaders = new();
        private readonly List<string> _allowedMethods = new();
        private bool _allowCredentials;

        /// <summary>
        /// Allows any origin to access the resource
        /// </summary>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        public CorsPolicyBuilder AllowAnyOrigin()
        {
            _allowedOrigins.Clear();
            _allowedOrigins.Add("*");
            return this;
        }

        /// <summary>
        /// Allows a specific origin to access the resource
        /// </summary>
        /// <param name="origin">The origin to allow</param>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when AllowAnyOrigin is already used</exception>
        public CorsPolicyBuilder AllowOrigin(string origin)
        {
            if (_allowedOrigins.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific origins when AllowAnyOrigin is used.");

            _allowedOrigins.Add(origin);
            return this;
        }

        /// <summary>
        /// Allows any header to be sent in the request
        /// </summary>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        public CorsPolicyBuilder AllowAnyHeader()
        {
            _allowedHeaders.Clear();
            _allowedHeaders.Add("*");
            return this;
        }

        /// <summary>
        /// Allows a specific header to be sent in the request
        /// </summary>
        /// <param name="header">The header to allow</param>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when AllowAnyHeader is already used</exception>
        public CorsPolicyBuilder AllowHeader(string header)
        {
            if (_allowedHeaders.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific headers when AllowAnyHeader is used.");

            _allowedHeaders.Add(header);
            return this;
        }

        /// <summary>
        /// Allows any method to be used in the request
        /// </summary>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        public CorsPolicyBuilder AllowAnyMethod()
        {
            _allowedMethods.Clear();
            _allowedMethods.Add("*");
            return this;
        }

        /// <summary>
        /// Allows a specific method to be used in the request
        /// </summary>
        /// <param name="method">The method to allow</param>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when AllowAnyMethod is already used</exception>
        public CorsPolicyBuilder AllowMethod(string method)
        {
            if (_allowedMethods.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific methods when AllowAnyMethod is used.");

            _allowedMethods.Add(method);
            return this;
        }

        /// <summary>
        /// Allows credentials to be included in the request
        /// </summary>
        /// <returns>The current instance of <see cref="CorsPolicyBuilder"/></returns>
        public CorsPolicyBuilder AllowCredentials()
        {
            _allowCredentials = true;
            return this;
        }

        /// <summary>
        /// Builds the CORS policy and applies it to the given response
        /// </summary>
        /// <param name="response">The HTTP response to apply the CORS policy to</param>
        public void Build(HttpListenerResponse response)
        {
            if (_allowedOrigins.Contains("*"))
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            else
                response.Headers.Add("Access-Control-Allow-Origin", string.Join(", ", _allowedOrigins));

            if (_allowedMethods.Contains("*"))
                response.Headers.Add("Access-Control-Allow-Methods", "*");
            else
                response.Headers.Add("Access-Control-Allow-Methods", string.Join(", ", _allowedMethods));

            if (_allowedHeaders.Contains("*"))
                response.Headers.Add("Access-Control-Allow-Headers", "*");
            else
                response.Headers.Add("Access-Control-Allow-Headers", string.Join(", ", _allowedHeaders));

            if (_allowCredentials)
                response.Headers.Add("Access-Control-Allow-Credentials", "true");
        }
    }
}


