
using System;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Attribute for POST routes
    /// </summary>
    public class PostAttribute : RouteAttribute
    {
        public PostAttribute(string path) : base(path, RequestMethod.Post) { }
    }
}
