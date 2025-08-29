using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_21_EmployeeOvertimeExceedingHoursReport : APIController
    {
        private readonly I_5_2_21_EmployeeOvertimeExceedingHoursReport _service;

        public C_5_2_21_EmployeeOvertimeExceedingHoursReport(I_5_2_21_EmployeeOvertimeExceedingHoursReport service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] EmployeeOvertimeExceedingHoursReportParam param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] EmployeeOvertimeExceedingHoursReportParam param)
        {
            param.UserName = userName;
            var result = await _service.Download(param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, roleList);
            return Ok(result);
        }
    }
}