using API.Dtos.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace API.Helpers.Utilities
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IJwtUtility
    {
        public string GenerateJwtToken(UsersDto user);
    }

    public class JwtUtility : IJwtUtility
    {

        public JwtUtility()
        {
        }

        public string GenerateJwtToken(UsersDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.UserData, user.UserName.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsConfigUtility.GetCurrentSettings("Appsettings:Token")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}