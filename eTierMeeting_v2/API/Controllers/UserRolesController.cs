using Machine_API._Service.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class UserRolesController : ApiController
    {
        private readonly IUserRolesService _userRolesService;
        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet("GetRoleByUser")]
        public async Task<IActionResult> GetRoleByUser(string user)
        {
            var result = await _userRolesService.GetRoleByUser(user);
            return Ok(result);
        }
    }
}