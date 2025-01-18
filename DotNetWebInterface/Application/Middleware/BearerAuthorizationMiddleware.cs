using DotNetWebInterface.Controllers;
using DotNetWebInterface.Server;
using DotNetWebInterface.Services;

namespace DotNetWebInterface.Authentication
{
    public class BearerAuthorizationMiddleware
    {
        private readonly AuthenticationService _authenticationService; 
        const string bearerPrefix = "Bearer ";

        public BearerAuthorizationMiddleware(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
         
        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return async (context, next) =>
            {
                if (ControllerResolver.IsAuthenticationRequired(context.AbsolutePath))
                {
                    var authHeader = context.Request.Headers["Authorization"];
                    if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Authorization header is missing or invalid");
                        await context.WriteAsync(401, "Unauthorized: Missing or invalid Authorization header");
                        return;
                    }

                    var token = authHeader.Substring(bearerPrefix.Length).Trim();

                    if (!_authenticationService.ValidateJwtToken(token, out var payload) || payload == null)
                    {
                        Console.WriteLine("JWT token is invalid");
                        context.Response.StatusCode = 401;
                        await context.WriteAsync(401, "Unauthorized: Invalid token");
                        return;
                    }
                      
                    if (payload is IDisposable disposablePayload)
                    {
                        context.AddDisposable(disposablePayload);
                    }

                    context.SetClaims(payload);
                }

                await next();
            };
        }
    }
}
