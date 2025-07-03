using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using eTierV2_API._Services.Interfaces;
//using System.Web.Http.Cors;

namespace eTierV2_API.Controllers
{
    //[EnableCors("AllowAllHeaders")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListUserPaging(string account, string isActive, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _userService.GetListUserPaging(account, isActive, pageNumber, pageSize);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDTO user)
        {
            var updateBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userService.AddUser(user, updateBy);
            if (result)
            {
                return NoContent();
            }

            throw new Exception("Fail Add User");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            var updateBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userService.UpdateUser(user, updateBy);
            if (result)
            {
                return NoContent();
            }

            throw new Exception("Fail Add User");
        }

        [HttpGet("roleuser/{account}")]
        public async Task<IActionResult> GetRoleByUser(string account)
        {
            var result = await _userService.GetRoleByUser(account);
            return Ok(result);
        }

        [HttpPost("roleuser/{account}")]
        public async Task<IActionResult> UpdateRoleByUser(string account, List<RoleByUserDTO> roles)
        {
            if (!(await _userService.CheckExistUser(account)))
            {
                return NotFound();
            }
            var updateBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _userService.UpdateRoleByUser(account, roles, updateBy);
            if (result)
            {
                return NoContent();
            }

            throw new Exception("Fail Add User");
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(UserForLoginDto user)
        {
            var result = await _userService.ChangePassword(user);
            return Ok(result);
        }
    }
}