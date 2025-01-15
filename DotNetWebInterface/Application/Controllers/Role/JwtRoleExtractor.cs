using DotNetWebInterface.Server; 
 
namespace DotNetWebInterface.Controllers.Role
{
    public class JwtRoleExtractor : IRoleProvider
    {
        public IEnumerable<string> GetRoles(HttpContext context, RoleOptions options)
        {
            var rolesClaim = context?.Claims?.FirstOrDefault(c => c.Type == options.RoleFieldName); 
            return rolesClaim?.Value.Split(',') ?? Enumerable.Empty<string>(); 
        }
    }
}
