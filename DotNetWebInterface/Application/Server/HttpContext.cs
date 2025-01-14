using System.Collections.Specialized;
using System.Net;
using System.Security.Claims;

namespace DotNetWebInterface.Server
{
    /// <summary>
    /// Represents the HTTP context for a request and response
    /// </summary>
    public class HttpContext
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
            using var writer = new StreamWriter(Response.OutputStream);
            await writer.WriteAsync(content);
        }
    }
}
