using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_20_NightShiftSubsidyMaintenanceController : APIController
    {
        private readonly I_7_1_20_NightShiftSubsidyMaintenance _service;
        public C_7_1_20_NightShiftSubsidyMaintenanceController(I_7_1_20_NightShiftSubsidyMaintenance service)
        {
            _service = service;
        }

        [HttpPost("Excute")]
        public async Task<IActionResult> Excute([FromBody] NightShiftSubsidyMaintenanceDto_Param param)
        {
            return Ok(await _service.ExcuteQuery(param, userName));
        }

        [HttpGet("GetFactory")]
        public async Task<IActionResult> GetFactory([FromQuery]string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetPermissionGroup")]
        public async Task<IActionResult> GetPermissionGroup([FromQuery] string factory, [FromQuery]string language)
        {
            return Ok( await _service.GetListPermissionGroup(factory, language));
        }

        [HttpGet("CheckData")]
        public async Task<IActionResult> CheckData([FromQuery] NightShiftSubsidyMaintenanceDto_Param param)
        {
            return Ok(await _service.CheckData(param));
        }
    }
}