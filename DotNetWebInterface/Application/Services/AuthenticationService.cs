using DotNetWebInterface.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetWebInterface.Services
{
    /// <summary>
    /// Provides authentication services including JWT token validation
    /// </summary>
    public class AuthenticationService 
    {
        /// <summary>
        /// Gets or sets the authentication scheme
        /// </summary>
        public string Scheme { get; set; } = "DefaultScheme";

        private byte[]? _keyBytes;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private TokenValidationParameters? _validationParameters;
        private readonly IApplicationBuilder _appBuilder;

        public AuthenticationService(IApplicationBuilder appBuilder)
        {
            _appBuilder = appBuilder;
        }

        /// <summary>
        /// Configures the service to use JWT authentication with the specified secret key
        /// </summary>
        /// <param name="secretKey">The secret key used to sign the JWT tokens.</param>
        public void UseJwt(string secretKey)
        { 
            _appBuilder.HttpServer.AddMiddleware<BearerAuthorizationMiddleware>(); 

            _keyBytes = Encoding.UTF8.GetBytes(secretKey);
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        }

        /// <summary>
        /// Validates the specified JWT token and extracts the claims
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <param name="claims">The claims extracted from the token if validation is successful</param>
        /// <returns>True if the token is valid; otherwise, false</returns>
        public bool ValidateJwtToken(string token, out IEnumerable<Claim>? claims)
        {
            claims = null;

            if (_validationParameters == null)
            {
                Console.WriteLine("Validation parameters are not initialized");
                return false;
            }

            try
            {
                var claimsPrincipal = _tokenHandler.ValidateToken(token, _validationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    claims = jwtToken.Claims;
                }

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Token validation failed");
                return false;
            }
        }
    }
}
