
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_2_2_EmergencyContactsSheetReport : APIController
    {
        private readonly I_4_2_2_EmergencyContactsSheetReport _service;

        public C_4_2_2_EmergencyContactsSheetReport(I_4_2_2_EmergencyContactsSheetReport service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] EmergencyContactsReportParam param)
        {
            var result = await _service.GetTotalRows(param, roleList);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] EmergencyContactsReportParam param)
        {
            var result = await _service.DownloadExcel(param, roleList, userName);
            return Ok(result);
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision([FromQuery] string lang)
        {
            return Ok(await _service.GetListDivision(lang));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string division, [FromQuery] string lang)
        {
            return Ok(await _service.GetListFactory(division, roleList, lang));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string lang)
        {
            return Ok(await _service.GetListDepartment(division, factory, lang));
        }

    }
}