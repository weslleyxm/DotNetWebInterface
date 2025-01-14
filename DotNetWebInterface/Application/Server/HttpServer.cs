using System.Net;
using DotNetWebInterface.Controllers;
using DotNetWebInterface.Cors;
using DotNetWebInterface.Middleware;
using DotNetWebInterface.Route;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetWebInterface.Server
{
    public class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly string _host;
        private MiddlewarePipeline pipeline;
        private CorsPolicyBuilder? corsPolicyBuilder;
        internal string RoutePrefix { get; private set; }

        public HttpServer(string host)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"{host}/");
            pipeline = new MiddlewarePipeline();
            RoutePrefix = string.Empty;
            _host = host;
        }

        internal void SetRoutePrefix(string prefix)
        {
            char charToAdd = '/';
            string newPrefix = prefix.StartsWith(charToAdd.ToString()) ? prefix : charToAdd + prefix;
            RoutePrefix = newPrefix;

            ControllerResolver.SetPrefix(RoutePrefix);
            ControllerResolver.ResolvePrefix();
        }

        internal void AddCorsPolicy(CorsPolicyBuilder corsPolicy)
        {
            corsPolicyBuilder = corsPolicy;
        }

        internal void AddMiddleware<T>() where T : class
        {
            var serviceProvider = ServiceContainer.Resolve<IServiceProvider>();
            var middleware = ActivatorUtilities.CreateInstance<T>(serviceProvider);

            var invokeMethod = typeof(T).GetMethod("Invoke", Type.EmptyTypes);

            if (invokeMethod == null || invokeMethod.ReturnType != typeof(Func<HttpContext, Func<Task>, Task>))
            {
                throw new InvalidOperationException($"{typeof(T).Name} must have an 'Invoke' method that returns Func<HttpContext, Func<Task>, Task>.");
            }

            pipeline.AddMiddleware(((dynamic)middleware).Invoke());
        }

        internal async Task RunAsync()
        {
            _listener.Start();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($", you are using {pipeline.Count} middleware{(pipeline.Count > 1 ? "s" : "")}");
            Console.WriteLine();

            Console.ResetColor();
            Console.WriteLine($"Server is running on {string.Join(", ", _listener.Prefixes)}");

            while (true)
            {
                var context = await _listener.GetContextAsync();
                Console.ResetColor();
                Request(context);
            }
        }

        private void Request(HttpListenerContext context)
        {
            _ = Task.Run(async () =>
            {
                var requestPath = $"{context.Request.Url?.AbsolutePath.ToLowerInvariant()}";

                var requestContext = new HttpContext(context.Request, context.Response, requestPath);
                corsPolicyBuilder?.Build(context.Response);

                try
                {
                    var httpMethod = Enum.TryParse(context.Request.HttpMethod, true, out RequestMethod method)
                        ? method
                        : RequestMethod.Get;

                    pipeline.SetRouteExecution(context => ControllerResolver.GetRouteExecution(context)(context));
                    await pipeline.Execute(requestContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    await requestContext.WriteAsync(404, "Internal Server Error");
                }
            });
        }
    }
}
