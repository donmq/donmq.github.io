using System.Security.Claims;
using API._Services.Interfaces.SeaHr;
using API.Dtos.Common;
using API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    public class SeaConfirmController : ApiController
    {
        private readonly ISeaConfirmService _seaHrSeaConfirmService;

        public SeaConfirmController(ISeaConfirmService seaHrSeaConfirmService)
        {
            _seaHrSeaConfirmService = seaHrSeaConfirmService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] SeaConfirmParam param, [FromQuery] PaginationParam pagination)
        {
            var result = await _seaHrSeaConfirmService.Search(param, pagination);
            return Ok(result);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _seaHrSeaConfirmService.GetCategories();
            return Ok(result);
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await _seaHrSeaConfirmService.GetDepartments();
            return Ok(result);
        }

        [HttpGet("Parts")]
        public async Task<IActionResult> GetParts([FromQuery] int deptID)
        {
            var result = await _seaHrSeaConfirmService.GetParts(deptID);
            return Ok(result);
        }

        [HttpGet("EmpDetail")]
        public async Task<IActionResult> GetEmpDetail([FromQuery] int empID)
        {
            var result = await _seaHrSeaConfirmService.GetEmpDetail(empID);
            return Ok(result);
        }
        
        [HttpGet("GetLeaveDeleteTopFive")]
        public async Task<IActionResult> GetLeaveDeleteTopFive([FromQuery] int empID)
        {
            var result = await _seaHrSeaConfirmService.GetLeaveDeleteTopFive(empID);
            return Ok(result);
        }

        [HttpPut("Confirm")]
        public async Task<IActionResult> Confirm([FromBody] List<LeaveDataDto> data)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _seaHrSeaConfirmService.Confirm(data, username);
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] SeaConfirmParam param, [FromQuery] PaginationParam pagination)
        {
            var result = await _seaHrSeaConfirmService.DownloadExcel(param, pagination);   
            return Ok(result);
        }

    }
}