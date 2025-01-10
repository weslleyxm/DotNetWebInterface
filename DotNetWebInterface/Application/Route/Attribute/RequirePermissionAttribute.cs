namespace DotNetWebInterface.Application.Route
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequirePermissionAttribute : Attribute
    {
        public string[] RequirePermissions { get; set; }

        public RequirePermissionAttribute(params string[] requirePermissions)
        {
            RequirePermissions = requirePermissions;
        }
    }
}