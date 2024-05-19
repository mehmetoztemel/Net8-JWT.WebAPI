using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Net8_JWT.WebAPI.Models;
using Net8_JWT.WebAPI.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Net8_JWT.WebAPI.Services
{
    public sealed class JwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtProvider(IOptionsMonitor<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.CurrentValue;
        }

        public string CreateToken(AppUser appUser)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.FirstName),
                new Claim("FullName",string.Join("",appUser.FirstName,appUser.LastName))
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(securityToken);
        }
    }
}
