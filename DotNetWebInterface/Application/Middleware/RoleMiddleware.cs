using DotNetWebInterface.Controllers; 
using DotNetWebInterface.Server;

namespace DotNetWebInterface.Middleware
{
    internal class RoleMiddleware
    {
        private readonly IRoleProvider _roleProvider;
        private readonly IApplicationBuilder _applicationBuilder;

        public RoleMiddleware(IRoleProvider roleProvider, IApplicationBuilder application)
        {
            _roleProvider = roleProvider;
            _applicationBuilder = application; 
        }

        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return async (context, next) =>
            { 
                if (ControllerResolver.IsRoleRequired(context.AbsolutePath, out string requiredRole)) 
                {
                    var roleOptions = _applicationBuilder.GetConfiguration<RoleOptions>();

                    if (roleOptions == null)
                    { 
                        await context.WriteAsync(403, "Forbidden: role options not configured");
                        return;
                    }

                    var roles = _roleProvider.GetRoles(context, roleOptions);

                    if (roles == null || !roles.Any())
                    {
                        await context.WriteAsync(403, "Forbidden: you do not have permission to access");
                        return;
                    }

                    var roleResolver = new RoleResolver(roleOptions);
                      
                    if(roleOptions.RoleLevels.Count <= 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black; 
                        Console.WriteLine("It looks like no levels are configured for the roles ensure levels are properly defined");
                        Console.ResetColor();

                        await context.WriteAsync(403, "Forbidden: insufficient role level");
                        return;
                    }
                
                    if (!roleResolver.HasRequiredRoleLevel(roles, requiredRole)) 
                    {
                        await context.WriteAsync(403, "Forbidden: insufficient role level");
                        return;
                    }
                }

                await next();
            };
        }
    }
}
