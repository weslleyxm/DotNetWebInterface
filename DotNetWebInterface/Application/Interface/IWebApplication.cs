using DotNetWebInterface.Application.Cors;
using System;

namespace DotNetWebInterface.Application
{
    public interface IWebApplication
    {
        public Task Run();
        public void UseMiddleware<T>() where T : class; 
        public void UseCors(Action<CorsPolicyBuilder> action); 
        void SetRoutePrefix(string prefix); 
    }
}
