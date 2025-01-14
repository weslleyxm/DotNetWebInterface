using DotNetWebInterface.Server; 

namespace DotNetWebInterface.Services
{
    public class BaseService : IService
    {
        public string Name { get; set; } = "Unnamed Service";

        public virtual void Initialize(IServiceContainer services)
        {

        }

        public virtual void HandleRequest(HttpContext context)
        {

        }
    }
}
