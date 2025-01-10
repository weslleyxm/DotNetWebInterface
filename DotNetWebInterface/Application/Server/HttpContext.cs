using System.Collections.Specialized;
using System.Net;
using System.Security.Claims;

namespace DotNetWebInterface.Application.Core
{
    public class HttpContext
    {
        internal HttpListenerRequest Request { get; }
        internal HttpListenerResponse Response { get; }
        public string AbsolutePath { get; }
        internal IEnumerable<Claim>? Claims { get; set; }
        internal NameValueCollection? QueryString { get; set; }
          
        public HttpContext(HttpListenerRequest request, HttpListenerResponse response, string absolutePath)
        {
            Request = request;
            Response = response;
            AbsolutePath = absolutePath; 
            QueryString = request.QueryString;
        }

        public void SetClaims(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }

        public async Task WriteAsync(int code, string content, string contentType = "text/plain")
        {
            Response.StatusCode = code;
            Response.ContentType = contentType;
            using var writer = new StreamWriter(Response.OutputStream);
            await writer.WriteAsync(content);
        }
    }
}
