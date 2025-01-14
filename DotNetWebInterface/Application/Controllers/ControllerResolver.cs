using System.Reflection;
using DotNetWebInterface.Route;
using DotNetWebInterface.Server;

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// The ControllerResolver class is responsible for mapping and resolving controllers for the application
    /// It maintains a dictionary of routes and provides methods to map controllers, set a prefix for routes,
    /// resolve the prefix, get the controller execution function, and check if authentication is required for a route
    /// </summary>
    public static class ControllerResolver
    {
        private static readonly Dictionary<string, RouteInfo> _routes = new();
        private static string Prefix = string.Empty;

        /// <summary>
        /// Maps the controllers by scanning the assembly for controllers and their methods with RouteAttribute
        /// </summary>
        public static void Resolve()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var derivedTypes = assemblies
                               .SelectMany(assembly => assembly.GetTypes())
                               .Where(t => typeof(Controller).IsAssignableFrom(t) && !t.IsAbstract)
                               .ToList();

            foreach (var routeClass in derivedTypes)
            {
                var methods = routeClass.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.GetCustomAttributes<RouteAttribute>().Any());

                foreach (var method in methods)
                {
                    var routeAttribute = method.GetCustomAttribute<RouteAttribute>();
                    if (routeAttribute != null)
                    {
                        var routeKey = routeAttribute.Path.ToLowerInvariant();

                        if (_routes.ContainsKey(routeKey))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Duplicate route detected: {routeKey}");
                            Console.ResetColor();
                            continue;
                        }

                        var authenticationRequired = false;
                        var roleRequired = false;

                        var authenticationAttribute = method.GetCustomAttribute<RequireAuthenticationAttribute>();
                        if (authenticationAttribute != null)
                        {
                            authenticationRequired = true;
                        }

                        string strRoleRequire = "";
                        var roleAttribute = method.GetCustomAttribute<RequireRoleAttribute>();
                        if (roleAttribute != null)
                        {
                            strRoleRequire = roleAttribute.RequiredRole;
                            roleRequired = true;
                        }

                        ParameterInfo[] parameters = method.GetParameters();
                        var requestType = parameters.Length > 0 ? parameters[0].ParameterType : typeof(object);

                        _routes.Add(routeKey, new RouteInfo(routeAttribute.Method, method, routeClass, requestType, authenticationRequired, roleRequired, strRoleRequire));
                    }
                }
            }

            Console.Write($"{_routes.Count} route{(_routes.Count > 1 ? "s" : "")} found");
        }

        /// <summary>
        /// Sets the prefix for the controller
        /// </summary>
        /// <param name="prefix">The prefix to set</param>
        public static void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        /// <summary>
        /// Resolves the prefix for the routes by updating the keys in the controller dictionary
        /// </summary>
        public static void ResolvePrefix()
        {
            foreach (var item in _routes.ToList())
            {
                var value = _routes[item.Key];

                _routes.Remove(item.Key);
                _routes.Add($"{Prefix}{item.Key}", value);
            }
        }

        /// <summary>
        /// Gets the controller execution function for the given HttpContext
        /// </summary>
        /// <param name="context">The HttpContext for the request</param>
        /// <returns>A function that executes the route.</returns>
        public static Func<HttpContext, Task> GetRouteExecution(HttpContext context)
        {
            var httpMethod = Enum.TryParse(context.Request.HttpMethod, true, out RequestMethod method)
                ? method
                : RequestMethod.Get;

            if (_routes.TryGetValue(context.AbsolutePath, out var route) && route.Method == httpMethod)
            {
                return async ctx =>
                {
                    try
                    {
                        if (route.Action != null && route != null)
                        {
                            var instance = Factory.Create<Controller>(route.Type);
                            instance.SetContext(context);

                            var returnType = route.Action.ReturnType;

                            var parameters = RequestResolver.Resolver(route, context);

                            if (typeof(Task).IsAssignableFrom(returnType))
                            {
                                var task = (Task)route.Action.Invoke(instance, parameters)!;
                                await task;
                            }
                            else
                            {
                                route.Action.Invoke(instance, parameters);
                            }

                            instance.Dispose();
                        }
                        else
                        {
                            await ctx.WriteAsync(500, "Internal Server Error: Route action or instance is null");
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        Console.WriteLine($"Route Execution Error: {ex.InnerException?.Message}");
                        await ctx.WriteAsync(500, "Internal Server Error: Exception during route execution");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected Error: {ex.Message}");
                        await ctx.WriteAsync(500, "Internal Server Error");
                    }
                };
            }
            else
            {
                return async ctx =>
                {
                    await ctx.WriteAsync(404, "Route not found");
                };
            }
        }

        /// <summary>
        /// Checks if authentication is required for the given request path
        /// </summary>
        /// <param name="requestPath">The request path to check</param>
        /// <returns>True if authentication is required, otherwise false</returns>
        internal static bool IsAuthenticationRequired(string requestPath)
        {
            return _routes.TryGetValue(requestPath, out var route) && route.AuthenticationRequired;
        }

        internal static bool IsRoleRequired(string requestPath, out string roleRequired)
        {
            roleRequired = string.Empty;
            if (_routes.TryGetValue(requestPath, out var route) && route.RoleIsRequired)
            {
                roleRequired = route.RoleRequired;
                return true;
            }

            return false;
        }

    }
}
