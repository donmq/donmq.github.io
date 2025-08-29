using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;
using SDCores;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_9_MonthlyAttendanceSetting: APIController
    {
        private readonly I_5_1_9_MonthlyAttendanceSetting _service;
        public C_5_1_9_MonthlyAttendanceSetting(I_5_1_9_MonthlyAttendanceSetting service)
        {
            _service = service;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] MonthlyAttendanceSettingParam param)
        {
            var result = await _service.GetDataPagination(pagination, param, userName);
            return Ok(result);
        }

        [HttpGet("GetFactorys")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, userName);
            return Ok(result);
        }

        [HttpGet("GetLeaveTypes")]
        public async Task<IActionResult> GetLeaveTypes(string language)
        {
            var result = await _service.GetLeaveTypes(language);
            return Ok(result);
        }

        [HttpGet("GetAllowances")]
        public async Task<IActionResult> GetAllowances(string language)
        {
            var result = await _service.GetAllowances(language);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<HRMS_Att_Use_Monthly_LeaveDto> models)
        {
            var result = await _service.Create(models, userName);
            return Ok(result);
        }

        [HttpGet("GetCloneData")]
        public async Task<IActionResult> GetCloneData(string factory, string leave_Type, string effective_Month)
        {
            var result = await _service.GetCloneData(factory, leave_Type, effective_Month, userName);
            return Ok(result);
        }
        [HttpGet("GetRecentData")]
        public async Task<IActionResult> GetRecentData(string factory, string effective_Month, string leave_Type, string action)
        {
            var result = await _service.GetRecentData(factory, effective_Month, leave_Type, action);
            return Ok(result);
        }
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] List<HRMS_Att_Use_Monthly_LeaveDto> models)
        {
            var result = await _service.Edit(models, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string factory, string effective_Month)
        {
            var result = await _service.Delete(factory, effective_Month);
            return Ok(result);
        }

        [HttpGet("CheckDuplicateEffectiveMonth")]
        public async Task<IActionResult> CheckDuplicateEffectiveMonth(string factory, string effective_Month)
        {
            var result = await _service.CheckDuplicateEffectiveMonth(factory, effective_Month);
            return Ok(result);
        }
    }
}