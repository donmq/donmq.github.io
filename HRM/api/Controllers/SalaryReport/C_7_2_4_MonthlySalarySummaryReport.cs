using API._Services.Interfaces.SalaryReport;
using API.DTOs.SalaryReport;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryReport
{
    public class C_7_2_4_MonthlySalarySummaryReportController : APIController
    {
        private readonly I_7_2_4_MonthlySalarySummaryReport _service;

        public C_7_2_4_MonthlySalarySummaryReportController(I_7_2_4_MonthlySalarySummaryReport service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }

        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel(string language)
        {
            return Ok(await _service.GetListLevel(language));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetTotal")]
        public async Task<IActionResult> GetTotal([FromQuery] MonthlySalarySummaryReportParam param)
        {
            return Ok(await _service.GetTotal(param));
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] MonthlySalarySummaryReportParam param)
        {
            return Ok(await _service.DownloadExcel(param, userName));
        }
    }
}