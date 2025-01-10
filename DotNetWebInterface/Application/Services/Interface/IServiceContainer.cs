namespace DotNetWebInterface.Application.Services
{
    public interface IServiceContainer 
    { 
        IServiceProvider _serviceProvider { get; } 
        T GetService<T>() where T : IService; 
        void AddService<T>(T service) where T : IService;
    }
}
