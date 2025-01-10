namespace DotNetWebInterface.Application.Route
{
    /// <summary>
    /// Attribute to define a route for a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// Gets the route string
        /// </summary>
        internal string Path { get; }

        /// <summary>
        /// Gets the HTTP method
        /// </summary>
        internal RequestMethod Method { get; } 

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteAttribute"/> class
        /// </summary>
        /// <param name="path">The path string</param>
        /// <param name="method">The HTTP method</param>
        public RouteAttribute(string path, RequestMethod method = RequestMethod.Get)
        {
            Path = path;
            Method = method; 
        }
    }
}