namespace DotNetWebInterface.Services
{
    public interface IServiceContainer
    {
        IService _serviceProvider { get; }
        T GetService<T>() where T : IService;
        void AddService<T>(T service) where T : IService;
    }
}
