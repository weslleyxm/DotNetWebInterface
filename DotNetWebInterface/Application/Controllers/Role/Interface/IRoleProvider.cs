using DotNetWebInterface.Server;

namespace DotNetWebInterface
{
    public interface IRoleProvider 
    {
        IEnumerable<string> GetRoles(HttpContext context, RoleOptions options);
    }
}
