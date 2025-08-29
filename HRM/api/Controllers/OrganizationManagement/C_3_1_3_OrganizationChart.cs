using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizationManagement
{
    public class C_3_1_3_OrganizationChart : APIController
    {
        private readonly I_3_1_3_OrganizationChart _service;
        public C_3_1_3_OrganizationChart(I_3_1_3_OrganizationChart service)
        {
            _service = service;
        }
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList([FromQuery] OrganizationChartParam param)
        {
            var result = await _service.GetDropDownList(param);
            return Ok(result);
        }
        [HttpGet("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList([FromQuery] OrganizationChartParam param)
        {
            var result = await _service.GetDepartmentList(param);
            return Ok(result);
        }
        [HttpGet("GetChartData")]
        public async Task<IActionResult> GetChartData([FromQuery] OrganizationChartParam param)
        {
            var data = await _service.GetChartData(param);
            return Ok(data);
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] OrganizationChartParam param)
        {
            var result = await _service.DownloadExcel(param);
            return Ok(result);
        }
    }
}