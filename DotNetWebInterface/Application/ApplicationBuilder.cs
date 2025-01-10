using DotNetWebInterface.Application.Core;
using DotNetWebInterface.Application.Route;
using DotNetWebInterface.Config;

namespace DotNetWebInterface.Application.Builder
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private HttpServer? HttpServer { get; set; }

        public static ApplicationBuilder Create()
        {
            return new ApplicationBuilder();
        }

        public WebApplication Build()
        {
            HttpServer = new HttpServer(GetPrefixes());

            RouteResolver.Map();

            WebApplication webApplication = new WebApplication(HttpServer);
            return webApplication;
        }

        public string GetPrefixes()
        {
            string prefix = LaunchSettings.ApplicationUrl ?? "http://localhost";
            return prefix;
        }
    }
}
