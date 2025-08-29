
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_22_MonthlySalaryGenerationController : APIController
    {
        private readonly I_7_1_22_MonthlySalaryGeneration _service;

        public C_7_1_22_MonthlySalaryGenerationController(I_7_1_22_MonthlySalaryGeneration service)
        {
            _service = service;
        }

        [HttpGet("CheckData")]
        public async Task<IActionResult> CheckData([FromQuery] MonthlySalaryGenerationParam param)
        {
            return Ok(await _service.CheckData(param));
        }

        [HttpPut("MonthlySalaryGenerationExecute")]
        public async Task<IActionResult> MonthlySalaryGenerationExecute([FromBody] MonthlySalaryGenerationParam param)
        {
            param.UserName = userName;
            return Ok(await _service.MonthlySalaryGenerationExecute(param));
        }

        [HttpPut("MonthlyDataLockExecute")]
        public async Task<IActionResult> MonthlyDataLockExecute([FromBody] MonthlyDataLockParam param)
        {
            param.UserName = userName;
            return Ok(await _service.MonthlyDataLockExecute(param));
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