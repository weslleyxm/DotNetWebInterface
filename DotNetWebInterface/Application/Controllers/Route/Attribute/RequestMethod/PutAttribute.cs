
using System;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Attribute for PUT routes
    /// </summary>
    public class PutAttribute : RouteAttribute
    {
        public PutAttribute(string path) : base(path, RequestMethod.Put) { }
    }
}
