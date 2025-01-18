using System.Collections.Specialized; 
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DotNetWebInterface.Server
{
    /// <summary>
    /// Represents the HTTP context for a request and response
    /// </summary>
    public class HttpContext : IDisposable
    {
        /// <summary>
        /// Gets the HTTP request
        /// </summary>
        internal HttpListenerRequest Request { get; }

        /// <summary>
        /// Gets the HTTP response
        /// </summary>
        internal HttpListenerResponse Response { get; }

        /// <summary>
        /// Gets or sets the parameters associated with the request
        /// </summary> 
        internal Dictionary<string, string>? Parameters { get; set; }

        /// <summary>
        /// Gets the absolute path of the request
        /// </summary>
        public string AbsolutePath { get; }

        /// <summary>
        /// Gets or sets the claims associated with the request
        /// </summary>
        internal IEnumerable<Claim>? Claims { get; set; }

        /// <summary>
        /// Gets or sets the query string parameters of the request
        /// </summary>
        internal NameValueCollection? QueryString { get; set; }

        /// <summary>
        /// Gets or sets the files associated with the request
        /// </summary>
        internal IEnumerable<string>? Files { get; set; }
         
        internal readonly DisposablesManager _disposablesManager = new(); 

        /// <summary>
        /// Indicates whether the object has been disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContext"/> class
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <param name="response">The HTTP response</param>
        /// <param name="absolutePath">The absolute path of the request</param>
        public HttpContext(HttpListenerRequest request, HttpListenerResponse response, string absolutePath)
        {
            Request = request;
            Response = response;
            AbsolutePath = absolutePath;
            QueryString = request.QueryString; 
        }

        internal void AddDisposable(IDisposable disposable)
        {
            _disposablesManager.Add(disposable);
        }
         
        /// <summary>
        /// Sets the claims associated with the request
        /// </summary>
        /// <param name="claims">The claims to set</param>
        public void SetClaims(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }

        /// <summary>
        /// Writes the specified content to the response asynchronously
        /// </summary>
        /// <param name="code">The HTTP status code</param>
        /// <param name="content">The content to write</param>
        /// <param name="contentType">The content type Default is "text/plain"</param>
        /// <returns>A task that represents the asynchronous write operation</returns>
        public async Task WriteAsync(int code, string content, string contentType = "text/plain")
        {
            Response.StatusCode = code;
            Response.ContentType = contentType;
              
            using var writer = new StreamWriter(Response.OutputStream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true);
            await writer.WriteAsync(content); 
        }

        /// <summary>
        /// Releases the unmanaged resources used by the HttpContext and optionally releases the managed resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposablesManager.DisposeAll();
                    Request?.InputStream?.Dispose();
                    Response?.OutputStream?.Close();
                }  

                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes the current instance
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor to ensure resources are released if Dispose is not called
        /// </summary>
        ~HttpContext()
        {
            Dispose(disposing: false);
        }
    }
}
