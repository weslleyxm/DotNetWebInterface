using DotNetWebInterface.Cors;
using System;

namespace DotNetWebInterface
{
    public interface IWebApplication
    {
        public Task Run();
        public void UseMiddleware<T>() where T : class;
        public void UseCors(Action<CorsPolicyBuilder> action);
        void SetRoutePrefix(string prefix);
    }
}
