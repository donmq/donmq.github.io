using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport : APIController
    {
        private readonly I_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport _service;

        public C_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport(I_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport service)
        {
            _service = service;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] ResignationAnnualLeaveWorktypeAnalysisReportParam param)
        {
            var result = await _service.Search(param);
            return Ok(result);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] ResignationAnnualLeaveWorktypeAnalysisReportParam param)
        {
            var result = await _service.DownloadExcel(param,userName);
            return Ok(result);
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, userName);
            return Ok(result);
        }

        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel(string language)
        {
            var result = await _service.GetListLevel(language);
            return Ok(result);
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string factory, [FromQuery] string language)
        {
            var result = await _service.GetListPermissionGroup(factory, language);
            return Ok(result);
        }
    }
}