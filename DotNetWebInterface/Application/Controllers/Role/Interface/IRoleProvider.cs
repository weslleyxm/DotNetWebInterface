using DotNetWebInterface.Server;

namespace DotNetWebInterface.Controllers.Role
{
    public interface IRoleProvider 
    {
        IEnumerable<string> GetRoles(HttpContext context, RoleOptions options);
    }
}
