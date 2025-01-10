using DotNetWebInterface.Application.Core; 

namespace DotNetWebInterface.Application.Services
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
