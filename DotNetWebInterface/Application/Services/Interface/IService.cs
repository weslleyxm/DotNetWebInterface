using DotNetWebInterface.Application.Core;

namespace DotNetWebInterface.Application.Services
{
    public interface IService
    {
        string Name { get; } 
        void Initialize(IServiceContainer services);  
        void HandleRequest(HttpContext context); 
    } 
}
