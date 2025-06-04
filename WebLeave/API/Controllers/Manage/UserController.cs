
using API._Services.Interfaces.Manage;
using Microsoft.AspNetCore.Mvc;
using API.Dtos.Manage.UserManage;
namespace API.Controllers.Manage
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int userId)
        {
            return Ok(await _userService.GetUser(userId));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParam pagination, string keyword)
        {
            return Ok(await _userService.GetAll(pagination, keyword));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UserForDetailDto usersDto)
        {
            return Ok(await _userService.Add(usersDto));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] UserForDetailDto usersDto)
        {
            return Ok(await _userService.Edit(usersDto));
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            return Ok(await _userService.UploadExcel(file));
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] UserManageTitleExcel title, string keyword, string lang)
        {
            var result = await _userService.DownloadExcel(title, keyword, lang);
            return Ok(result);
        }
    }
}