using API._Services.Interfaces.Manage;
using API.Dtos;
using API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{

    public class DepartmentController : ApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAllAreas")]
        public async Task<IActionResult> GetAllAreas()
        {
            var area = await _departmentService.GetAllAreas();
            return Ok(area);
        }

        [HttpGet("GetAllBuildings")]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _departmentService.GetAllBuildings();
            return Ok(buildings);
        }

        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments([FromQuery] PaginationParam pagination, [FromQuery] DepartmentParams search)
        {
            var Departments = await _departmentService.GetAllDepartment(pagination, search);
            return Ok(Departments);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DepartmentDto departmentDto)
        {
            var addDepartments = await _departmentService.Add(departmentDto);
            return Ok(addDepartments);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] DepartmentDto departmentDto)
        {
            var updateDepartments = await _departmentService.Update(departmentDto);
            return Ok(updateDepartments);
        }
    }
}