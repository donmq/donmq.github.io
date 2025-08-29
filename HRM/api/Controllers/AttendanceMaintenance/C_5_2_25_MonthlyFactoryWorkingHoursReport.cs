using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_25_MonthlyFactoryWorkingHoursReport : APIController
    {
        private readonly I_5_2_25_MonthlyFactoryWorkingHoursReport _service;

        public C_5_2_25_MonthlyFactoryWorkingHoursReport(I_5_2_25_MonthlyFactoryWorkingHoursReport service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] MonthlyFactoryWorkingHoursReportParam param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] MonthlyFactoryWorkingHoursReportParam param)
        {
            param.UserName = userName;
            var result = await _service.Download(param);
            return Ok(result);
        }

        #region Get List
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, roleList);
            return Ok(result);
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string factory, [FromQuery] string language)
        {
            var result = await _service.GetListPermissionGroup(factory, language);
            return Ok(result);
        }
        #endregion
    }
}