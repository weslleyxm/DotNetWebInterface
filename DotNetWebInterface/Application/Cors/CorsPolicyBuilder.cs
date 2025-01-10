using System.Net;

namespace DotNetWebInterface.Application.Cors
{
    public class CorsPolicyBuilder
    {
        private readonly List<string> _allowedOrigins = new();
        private readonly List<string> _allowedHeaders = new();
        private readonly List<string> _allowedMethods = new();
        private bool _allowCredentials;

        public CorsPolicyBuilder AllowAnyOrigin()
        {
            _allowedOrigins.Clear();
            _allowedOrigins.Add("*");
            return this;
        }

        public CorsPolicyBuilder AllowOrigin(string origin)
        {
            if (_allowedOrigins.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific origins when AllowAnyOrigin is used.");

            _allowedOrigins.Add(origin);
            return this;
        }

        public CorsPolicyBuilder AllowAnyHeader()
        {
            _allowedHeaders.Clear();
            _allowedHeaders.Add("*");
            return this;
        }

        public CorsPolicyBuilder AllowHeader(string header)
        {
            if (_allowedHeaders.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific headers when AllowAnyHeader is used.");

            _allowedHeaders.Add(header);
            return this;
        }

        public CorsPolicyBuilder AllowAnyMethod()
        {
            _allowedMethods.Clear();
            _allowedMethods.Add("*");
            return this;
        }

        public CorsPolicyBuilder AllowMethod(string method)
        {
            if (_allowedMethods.Contains("*"))
                throw new InvalidOperationException("Cannot allow specific methods when AllowAnyMethod is used.");

            _allowedMethods.Add(method);
            return this;
        }

        public CorsPolicyBuilder AllowCredentials()
        {
            _allowCredentials = true;
            return this;
        } 

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


