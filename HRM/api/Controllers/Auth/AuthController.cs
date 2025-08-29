using API._Services.Interfaces.Auth;
using API.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly I_Auth _iAuth;

        public AuthController(I_Auth iAuth)
        {
            _iAuth = iAuth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginParam param)
        {
            var data = await _iAuth.Login(param);
            if (data == null)
                return Unauthorized();
            return Ok(data);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory()
        {
            return Ok(await _iAuth.GetListFactory());
        }

        [HttpGet("GetDirection")]
        public async Task<IActionResult> GetDirection()
        {
            return Ok(await _iAuth.GetDirection());
        }

        [HttpGet("GetProgram")]
        public async Task<IActionResult> GetProgram([FromQuery] string direction)
        {
            return Ok(await _iAuth.GetProgram(direction));
        }
        [HttpGet("GetListLangs")]
        public async Task<IActionResult> GetListLangs()
        {
            return Ok(await _iAuth.GetListLangs());
        }

    }
}