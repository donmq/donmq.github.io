using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_21_MenstrualLeaveHoursAllowanceController : APIController
    {
        private readonly I_7_1_21_MenstrualLeaveHoursAllowance _service;

        public C_7_1_21_MenstrualLeaveHoursAllowanceController(I_7_1_21_MenstrualLeaveHoursAllowance service)
        {
            _service = service;
        }

        [HttpGet("CheckData")]
        public async Task<IActionResult> CheckData([FromQuery] MenstrualLeaveHoursAllowanceParam param)
        {
            return Ok(await _service.CheckData(param));
        }

        [HttpPut("Execute")]
        public async Task<IActionResult> Execute([FromBody] MenstrualLeaveHoursAllowanceParam param)
        {
            param.UserName = userName;
            return Ok(await _service.Execute(param));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(userName, language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }
    }
}