using DotNetWebInterface.Application.Services;

namespace DotNetWebInterface.Application
{
    public interface IApplicationBuilder
    { 
        public WebApplication Build(); 
        public string GetPrefixes();
    }
}
