
using System;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Attribute for DELETE routes
    /// </summary>
    public class DeleteAttribute : RouteAttribute
    {
        public DeleteAttribute(string path) : base(path, RequestMethod.Delete) { }
    }
}
