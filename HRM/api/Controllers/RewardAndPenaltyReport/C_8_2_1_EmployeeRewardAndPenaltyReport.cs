using API._Services.Interfaces.RewardAndPenaltyMaintenance;
using API.DTOs.RewardAndPenaltyReport;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RewardAndPenaltyReport
{
    public class C_8_2_1_EmployeeRewardAndPenaltyReport : APIController
    {
        private readonly I_8_2_1_EmployeeRewardAndPenaltyReport _service;

        public C_8_2_1_EmployeeRewardAndPenaltyReport(I_8_2_1_EmployeeRewardAndPenaltyReport service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] EmployeeRewardAndPenaltyReportParam param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] EmployeeRewardAndPenaltyReportParam param)
        {
            param.UserName = userName;
            var result = await _service.Download(param);
            return Ok(result);
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

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetListRewardPenalty")]
        public async Task<IActionResult> GetListRewardPenalty(string language)
        {
            return Ok(await _service.GetListRewardPenaltyType(language));
        }
    }
}