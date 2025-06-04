using API._Services.Interfaces.Manage;
using API.Dtos.Manage.TeamManagement;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{
    public class TeamManagementController : ApiController
    {
        private readonly ITeamManagementService _teamManagementService;

        public TeamManagementController(ITeamManagementService teamManagementService)
        {
            _teamManagementService = teamManagementService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PartDto partDto)
        {
            return Ok(await _teamManagementService.Create(partDto));
        }

        [HttpGet("GetAllDepartment")]
        public async Task<IActionResult> GetAllDepartment()
        {
            return Ok(await _teamManagementService.GetAllDepartment());
        }

        [HttpGet("GetDataPaginations")]
        public async Task<IActionResult> GetDataPaginations([FromQuery] PaginationParam pagination, string deptID, string partCode)
        {
            return Ok(await _teamManagementService.GetDataPaginations(pagination, deptID, partCode, true));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(PartDto partDto)
        {
            return Ok(await _teamManagementService.Update(partDto));
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportEcel([FromQuery] PaginationParam pagination,[FromQuery] PartParam param)
        {
            var result = await _teamManagementService.ExportExcel(pagination, param);
            return Ok(result);
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> GetDataDetail(int partID)
        {
            return Ok(await _teamManagementService.GetDataDetail(partID));
        }
    }
}