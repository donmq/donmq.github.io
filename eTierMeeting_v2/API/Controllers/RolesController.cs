using Machine_API._Service.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class RolesController : ApiController
    {
        private readonly IRolesService _rolesService;
        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;

        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _rolesService.GetAllRoles();
            return Ok(result);
        }
    }
}