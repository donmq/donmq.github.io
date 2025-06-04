using API._Services.Interfaces;
using API.Dtos.Auth;
using API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth
{
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginParam userForLogin)
        {
            // Kiểm tra hợp lệ tên đăng nhập và mật khẩu
            if (string.IsNullOrEmpty(userForLogin.Username?.Trim()) || string.IsNullOrEmpty(userForLogin.Password?.Trim()))
                return Ok(null);

            var userDto = await _authService.GetUser(userForLogin);
            if (userDto == null)
                return Ok(null);

            var isLoggedIn = await _authService.CheckLoggedIn(userForLogin);

            if (isLoggedIn && !userForLogin.ConfirmReLogin)
                return Ok(new LoginResponseDto { AlreadyLoggedIn = true });

            // Tiến hành đăng xuất
            await _authService.Logout(userForLogin);

            // Tiến hành đăng nhập
            var user = await _authService.Login(userForLogin, userDto);

            if (user == null)
                return Ok(null);

            return Ok(new LoginResponseDto
            {
                Token = user.Token,
                User = user
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] UserForLoginParam userForLogin)
        {
            if (!string.IsNullOrEmpty(userForLogin.Username?.Trim()) && !string.IsNullOrEmpty(userForLogin.IpLocal?.Trim()))
                await _authService.Logout(userForLogin);

            return NoContent();
        }

        [HttpGet("CountLeaveEdit")]
        public async Task<IActionResult> CountLeaveEdit([FromQuery] int userID)
        {
            var result = await _authService.CountLeaveEdit(userID);
            return Ok(result);
        }

        [HttpGet("CountSeaHrEdit")]
        public async Task<IActionResult> CountSeaHrEdit()
        {
            var result = await _authService.CountSeaHrEdit();
            return Ok(result);
        }

        [HttpGet("CountSeaHrConfirm")]
        public async Task<IActionResult> CountSeaHrConfirm()
        {
            var result = await _authService.CountSeaHrConfirm();
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserForLoginParam userForLogin)
        {
            var result = await _authService.ChangePassword(userForLogin);
            return Ok(result);
        }
    }
}