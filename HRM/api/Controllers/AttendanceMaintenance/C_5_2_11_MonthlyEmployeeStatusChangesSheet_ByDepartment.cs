using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment : APIController
    {
        private readonly I_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment _service;

        public C_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment(I_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] MonthlyEmployeeStatusParam param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] MonthlyEmployeeStatusParam param)
        {
            var result = await _service.DownloadExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string lang)
        {
            return Ok(await _service.GetListFactory(roleList, lang));
        }
        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel(string lang)
        {
            return Ok(await _service.GetListLevel(lang));
        }
        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string lang)
        {
            return Ok(await _service.GetListPermissionGroup(factory, lang));
        }
    }
}