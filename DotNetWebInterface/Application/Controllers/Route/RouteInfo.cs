using System.Reflection;

namespace DotNetWebInterface
{
    /// <summary>
    /// Represents the information about a route, including the HTTP method, action, type, request type, and authentication/authorization requirements
    /// </summary>
    public class RouteInfo(RequestMethod method, MethodInfo action, Type type, Type requestType, bool authenticationRequired = false, bool roleIsRequired = false, string roleRequired = "")
    {
        /// <summary>
        /// Gets or sets the HTTP method for the route
        /// </summary>
        public RequestMethod Method { get; set; } = method;

        /// <summary>
        /// Gets or sets the action method information for the route
        /// </summary>
        public MethodInfo Action { get; set; } = action ?? throw new ArgumentNullException(nameof(action));

        /// <summary>
        /// Gets or sets the type associated with the route
        /// </summary>
        public Type Type { get; set; } = type ?? throw new ArgumentNullException(nameof(type));

        /// <summary>
        /// Gets or sets a value indicating whether authentication is required for the route
        /// </summary>
        public bool AuthenticationRequired { get; set; } = authenticationRequired;

        /// <summary>
        /// Gets or sets a value indicating whether a specific role is required for the route
        /// </summary>
        public bool RoleIsRequired { get; set; } = roleIsRequired;

        /// <summary>
        /// Gets or sets the required role for the route
        /// </summary>
        public string RoleRequired { get; set; } = roleRequired;

        /// <summary>
        /// Gets or sets the request type for the route
        /// </summary>
        public Type RequestType { get; set; } = requestType;
    }
}
