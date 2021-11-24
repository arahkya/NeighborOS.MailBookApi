using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        [HttpGet]
        public string GetToken()
        {
            var issuerKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NTNv7j0TuYARvmNMmWXo6fKvM4o6nv/aUi9ryX38ZH+L1bkrnD1ObOQ8JAUmHCBq7Iy7otZcyAagBLHVKvvYaIpmMuxmARQ97jUVG16Jkpkp1wXOPsrF9zwew6TpczyHkHgX5EuLg2MeBuiT/qJACs1J0apruOOJCg/gOtkjB4c="));
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityTokenDescriptor = new SecurityTokenDescriptor();

            securityTokenDescriptor.Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, "Arahk"),
                new Claim(JwtRegisteredClaimNames.Aud, "mailbook.neighboros.in.th"),
                new Claim(JwtRegisteredClaimNames.Sub, "Arahk"),
                new Claim(JwtRegisteredClaimNames.Iss, "https://auth.neighboros.in.th"),
                new Claim(JwtRegisteredClaimNames.Email, "arahk@outlook.com")
            });

            securityTokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(10);
            securityTokenDescriptor.SigningCredentials = new SigningCredentials(issuerKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            var jwtSecurityTokenString = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return jwtSecurityTokenString;
        }
    }
}