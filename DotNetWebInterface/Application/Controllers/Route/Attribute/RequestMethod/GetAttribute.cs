
using System;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Attribute for GET routes
    /// </summary>
    public class GetAttribute : RouteAttribute
    {
        public GetAttribute(string path) : base(path, RequestMethod.Get) { }
    }
}
