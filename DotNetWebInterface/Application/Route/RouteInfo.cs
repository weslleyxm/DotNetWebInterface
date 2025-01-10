using Newtonsoft.Json;
using System.Reflection;

namespace DotNetWebInterface.Application.Route
{
    public class RouteInfo(RequestMethod method, MethodInfo action, Type type, Type requestType, bool authenticationRequired = false)
    {
        public RequestMethod Method { get; set; } = method;
        public MethodInfo Action { get; set; } = action ?? throw new ArgumentNullException(nameof(action));
        public Type Type { get; set; } = type ?? throw new ArgumentNullException(nameof(type)); 
        public bool AuthenticationRequired { get; set; } = authenticationRequired;
        public Type RequestType { get; set; } = requestType;
    }
}
