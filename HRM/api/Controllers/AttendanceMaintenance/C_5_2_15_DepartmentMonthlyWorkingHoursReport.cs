using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_15_DepartmentMonthlyWorkingHoursReport: APIController
    {
        private readonly I_5_2_15_DepartmentMonthlyWorkingHoursReport _service;
        public C_5_2_15_DepartmentMonthlyWorkingHoursReport(I_5_2_15_DepartmentMonthlyWorkingHoursReport service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] DepartmentMonthlyWorkingHoursReportParam param)
            => Ok(await _service.GetData(param, userName));

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _service.GetListFactory(language, userName));

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _service.GetListPermissionGroup(factory, language));

        [HttpGet("Excel")]
        public async Task<IActionResult> Excel([FromQuery] DepartmentMonthlyWorkingHoursReportParam param)
            => Ok(await _service.Excel(param, userName));

        
        
    }
}