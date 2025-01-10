using DotNetWebInterface.Application.Core;
using DotNetWebInterface.Application.Route;
using DotNetWebInterface.Application.Services;

namespace DotNetWebInterface.Application.Middleware
{
    public class BearerAuthorizationMiddleware
    {
        private readonly AuthenticationService _authenticationService;

        public BearerAuthorizationMiddleware(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return async (context, next) =>
            {
                if (RouteResolver.IsAuthenticationRequired(context.AbsolutePath))
                {
                    var authHeader = context.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    {
                        await context.WriteAsync(401, "Unauthorized: Missing or invalid Authorization header");
                        return;
                    }

                    var token = authHeader.Substring("Bearer ".Length).Trim();

                    if (!_authenticationService.ValidateJwtToken(token, out var payload) || payload == null)
                    {
                        context.Response.StatusCode = 401;
                        await context.WriteAsync(401, "Unauthorized: Invalid token");
                        return;
                    } 

                    context.SetClaims(payload); 
                }

                await next();
            };
        }
    }
}
