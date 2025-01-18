using System.Data;

namespace DotNetWebInterface.Controllers
{
    public class RoleResolver
    {
        private readonly RoleOptions _roleOptions;

        public RoleResolver(RoleOptions roleOptions)
        {
            _roleOptions = roleOptions;
        }

        public bool HasRequiredRoleLevel(IEnumerable<string> userRoles, string requiredRole)
        {
            var requiredLevel = _roleOptions.GetRoleLevel(requiredRole);
 
            return userRoles.Any(role =>
                _roleOptions.GetRoleLevel(role) >= requiredLevel);
        }
    }
}
