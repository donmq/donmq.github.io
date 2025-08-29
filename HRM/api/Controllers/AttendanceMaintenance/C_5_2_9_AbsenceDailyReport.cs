using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_9_AbsenceDailyReport : APIController
    {
        private readonly I_5_2_9_AbsenceDailyReport _service;
        public C_5_2_9_AbsenceDailyReport(I_5_2_9_AbsenceDailyReport service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] AbsenceDailyReportParam param)
        {
            var result = await _service.GetTotalRows(param, roleList);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] AbsenceDailyReportParam param)
        {
            var result = await _service.DownloadExcel(param, roleList, userName);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string lang)
        {
            return Ok(await _service.Queryt_Factory_AddList(userName, lang));
        }

    }
}