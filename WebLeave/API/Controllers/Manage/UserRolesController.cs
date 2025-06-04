
using API._Services.Interfaces.Manage;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{
    public class UserRolesController : ApiController
    {
        private readonly IUserRolesService _userRolesService;
        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel(int userId, string langId)
        {
            var result = await _userRolesService.DownloadExcel(userId, langId);
            return Ok(result);
        }

        [HttpGet("GetAllRoleUser")]
        public async Task<IActionResult> GetAllRoleUser(int userId, string langId)
        {
            var (Roles, AssignedRoles) = await _userRolesService.GetAllRoleUser(userId, langId);

            return Ok(new { Roles, AssignedRoles });
        }


        [HttpPost("AssignRole/{userId}")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            var result = await _userRolesService.AssignRole(userId, roleId, Convert.ToInt32(UserId));

            return Ok(result);
        }

        [HttpPost("UnAssignRole/{userId}")]
        public async Task<IActionResult> UnAssignRole(int userId, int roleId)
        {
            var result = await _userRolesService.UnAssignRole(userId, roleId);

            return Ok(result);
        }

        [HttpGet("GetAllGroupBase")]
        public async Task<IActionResult> GetAllGroupBase(int userId, string langId)
        {
            var (Roles, AssignedRoles) = await _userRolesService.GetAssignGroupBase(userId, langId);

            return Ok(new { Roles, AssignedRoles });
        }

        [HttpPost("AssignGroupBase")]
        public async Task<IActionResult> AssignGroupBase([FromQuery] int gbId, [FromQuery] int userId)
        {
            var result = await _userRolesService.AssignGroupBase(gbId, userId);

            return Ok(result);
        }

        [HttpPost("UnAssignGroupBase")]
        public async Task<IActionResult> UnAssignGroupBase([FromQuery] int gbId, [FromQuery] int userId)
        {
            var result = await _userRolesService.UnAssignGroupBase(gbId, userId);

            return Ok(result);
        }

        [HttpPost("UpdateRoleRank")]
        public async Task<IActionResult> UpdateRoleRank(int userId, int roleRank, bool isInherit)
        {
            var result = await _userRolesService.UpdateRoleRank(userId, roleRank, isInherit);

            return Ok(result);
        }

        [HttpPost("SetPermit")]
        public async Task<IActionResult> SetPermit(int userId, string key)
        {
            var result = await _userRolesService.SetPermit(userId, key, Convert.ToInt32(UserId));

            return Ok(result);
        }
        [HttpPost("RemovePermit")]
        public async Task<IActionResult> RemovePermit(int userId, string key)
        {
            var result = await _userRolesService.RemovePermit(userId, key);

            return Ok(result);
        }

        [HttpPost("SetReport")]
        public async Task<IActionResult> SetReport(int userId, string key)
        {
            var result = await _userRolesService.SetReport(userId, key, Convert.ToInt32(UserId));

            return Ok(result);
        }

        [HttpPost("RemoveReport")]
        public async Task<IActionResult> RemoveReport(int userId, string key)
        {
            var result = await _userRolesService.RemoveReport(userId, key);

            return Ok(result);
        }

        [HttpGet("ListUsers")]
        public async Task<IActionResult> ListUsers(int roleID)
        {
            var result = await _userRolesService.ListUsers(roleID);

            return Ok(result);
        }
    }
}