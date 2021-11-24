using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthApi.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private JwtConfigure _jwtConfigure;

        public AuthController(IOptions<JwtConfigure> jwtTokenConfigOpt)
        {
            _jwtConfigure = jwtTokenConfigOpt.Value;
        }

        [HttpGet]
        public string GetToken()
        {
            var issuerKey = new SymmetricSecurityKey(_jwtConfigure.KeyBytes);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityTokenDescriptor = new SecurityTokenDescriptor();

            securityTokenDescriptor.Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, "Arahk"),
                new Claim(JwtRegisteredClaimNames.Aud, "mailbook.neighboros.in.th"),
                new Claim(JwtRegisteredClaimNames.Sub, "Arahk"),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtConfigure.Issuer),
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