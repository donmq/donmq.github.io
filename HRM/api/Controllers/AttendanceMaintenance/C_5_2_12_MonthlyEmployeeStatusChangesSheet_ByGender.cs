using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender : APIController
    {
        private readonly I_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender _service;

        public C_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender(I_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] MonthlyEmployeeStatusChangesSheet_ByGender_Param param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] MonthlyEmployeeStatusChangesSheet_ByGender_Param param)
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