using System.Reflection;
using DotNetWebInterface.Application.Core;
using DotNetWebInterface.Application.Dependency;

namespace DotNetWebInterface.Application.Route
{

    public static class RouteResolver
    {
        private static readonly Dictionary<string, RouteInfo> _routes = new();
        private static string Prefix = string.Empty;

        public static void Map()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var assembly = Assembly.GetExecutingAssembly();
            var derivedTypes = assembly.GetTypes()
                .Where(t => typeof(BaseRoute).IsAssignableFrom(t) && !t.IsAbstract)
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

                        var authenticationAttribute = method.GetCustomAttribute<RequireAuthenticationAttribute>();
                        if (authenticationAttribute != null)
                        {
                            authenticationRequired = true;
                        }

                        ParameterInfo[] parameters = method.GetParameters();
                        var requestType = parameters.Length > 0 ? parameters[0].ParameterType : typeof(object);

                        _routes.Add(routeKey, new RouteInfo(routeAttribute.Method, method, routeClass, requestType, authenticationRequired)); 
                    }
                }
            }

            Console.Write($"{_routes.Count} route{(_routes.Count > 1 ? "s" : "")} found");
        }

        public static void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public static void ResolvePrefix()
        {
            foreach (var item in _routes.ToList())
            {
                var value = _routes[item.Key];

                _routes.Remove(item.Key);
                _routes.Add($"{Prefix}{item.Key}", value);
            }
        }

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
                            var instance = Factory.Create<BaseRoute>(route.Type);
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

        internal static bool IsAuthenticationRequired(string requestPath)
        {
            return _routes.TryGetValue(requestPath, out var route) && route.AuthenticationRequired;
        }
    }
}
