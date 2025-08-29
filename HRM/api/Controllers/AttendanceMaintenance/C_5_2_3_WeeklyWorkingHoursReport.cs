using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_3_WeeklyWorkingHoursReport : APIController
    {
        private readonly I_5_2_3_WeeklyWorkingHoursReport _service;

        public C_5_2_3_WeeklyWorkingHoursReport(I_5_2_3_WeeklyWorkingHoursReport service)
        {
            _service = service;
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] WeeklyWorkingHoursReportParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("GetCountRecords")]
        public async Task<IActionResult> GetCountRecords([FromQuery] WeeklyWorkingHoursReportParam param)
        {
            var result = await _service.GetCountRecords(param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(roleList, language));
        }

        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel(string language)
        {
            return Ok(await _service.GetListLevel(language));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language,factory));
        }
    }
}