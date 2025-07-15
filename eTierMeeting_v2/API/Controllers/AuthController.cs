using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Machine_API._Services.Interface;
using Machine_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Machine_API.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto model)
        {
            var userFromRepo = await _authService.Login(model);
            if (userFromRepo == null || userFromRepo.Visible == false)
                return Unauthorized();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.UserID.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName),
                new Claim("EmpName", userFromRepo.EmpName),
                new Claim("UserName", userFromRepo.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = userFromRepo,
            });
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword, string lang)
        {
            var data = await _authService.ChangePassword(changePassword, lang);
            return Ok(data);
        }

        [HttpGet("ServerInfo")]
        public async Task<IActionResult> GetServerInfo()
        {
            var factory = _configuration.GetSection("AppSettings:Factory").Value;
            var area = _configuration.GetSection("AppSettings:Area").Value;
            var local = string.Empty;

            switch (factory)
            {
                case "SHC":
                    local = "vi-VN";
                    break;

                case "CB":
                    local = "vi-VN";
                    break;

                case "TSH":
                    local = "id-ID";
                    break;

                default: // SPC
                    local = "en-US";
                    break;
            }

            var result = await Task.FromResult(new { factory, area, local });
            return Ok(result);
        }
    }
}