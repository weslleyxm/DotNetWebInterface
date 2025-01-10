using DotNetWebInterface.Application.Core;
using DotNetWebInterface.Application.Cors;

namespace DotNetWebInterface.Application
{
    public class WebApplication : IWebApplication
    {
        private readonly HttpServer httpServer;
        private CorsPolicyBuilder? corsPolicyBuilder;

        public WebApplication(HttpServer httpServer)
        {
            this.httpServer = httpServer;
        }

        public async Task Run()
        {
            await httpServer.RunAsync();
        }

        public void UseCors(Action<CorsPolicyBuilder> action)
        {
            corsPolicyBuilder = new CorsPolicyBuilder();
            httpServer.AddCorsPolicy(corsPolicyBuilder);

            action?.Invoke(corsPolicyBuilder);
        }

        void IWebApplication.UseMiddleware<T>() where T : class
        {
            httpServer.AddMiddleware<T>();
        }

        void IWebApplication.SetRoutePrefix(string prefix)
        {
            if (httpServer != null)
            {
                httpServer.SetRoutePrefix(prefix);
            }
            else
            {
                throw new InvalidOperationException("HttpServer is not initialized.");
            }
        }
    }
}
