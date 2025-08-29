using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_22_AnnualLeaveCalculation : APIController
    {
        private readonly I_5_2_22_AnnualLeaveCalculation _service;

        public C_5_2_22_AnnualLeaveCalculation(I_5_2_22_AnnualLeaveCalculation service)
        {
            _service = service;
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] AnnualLeaveCalculationParam param)
        {
            var result = await _service.GetTotalRows(param);
            return Ok(result);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] AnnualLeaveCalculationParam param)
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

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _service.GetListDepartment(factory, language);
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