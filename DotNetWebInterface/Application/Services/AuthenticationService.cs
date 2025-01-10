using DotNetWebInterface.Application.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetWebInterface.Application.Services
{
    public class AuthenticationService : BaseService
    {
        public string Scheme { get; set; } = "DefaultScheme";
        private readonly string _secretKey;

        public AuthenticationService()
        {
            _secretKey = "qwertyuiopasdfghjklzxcvbnm123456";
        }

        public bool ValidateJwtToken(string token, out IEnumerable<Claim>? claims)
        {
            claims = null;

            try
            { 
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);  

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                 
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                 
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    claims = jwtToken.Claims; 
                } 

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
        } 
    }
}
