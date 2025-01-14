using DotNetWebInterface.Server;

namespace DotNetWebInterface.Services
{
    public interface IService 
    {
        string Name { get; }
        void Initialize(IServiceContainer services);
        void HandleRequest(HttpContext context);
    }
}
