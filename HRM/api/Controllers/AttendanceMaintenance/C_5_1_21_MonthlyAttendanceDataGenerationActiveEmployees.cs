using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees : APIController
    {
        private readonly I_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees _service;
        public C_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees(I_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees service)
        {
            _service = service;
        }

        [HttpGet("CheckParam")]
        public async Task<IActionResult> CheckParam([FromQuery] GenerationActiveParam param)
        {
            return Ok(await _service.CheckParam(param));
        }

        [HttpPut("MonthlyAttendanceDataGeneration")]
        public async Task<IActionResult> MonthlyAttendanceDataGeneration([FromBody] GenerationActiveParam param)
        {
            param.UserName = userName;
            param.Current = DateTime.Now;
            return Ok(await _service.MonthlyAttendanceDataGeneration(param));
        }

        [HttpGet("SearchAlreadyDeadlineData")]
        public async Task<IActionResult> SearchAlreadyDeadlineData([FromQuery] PaginationParam pagination, [FromQuery] SearchAlreadyDeadlineDataParam param)
        {
            return Ok(await _service.SearchAlreadyDeadlineData(pagination, param));
        }

        [HttpPut("MonthlyDataCloseExecute")]
        public async Task<IActionResult> MonthlyDataCloseExecute([FromBody] MonthlyAttendanceDataGenerationActiveEmployees_MonthlyDataCloseParam param)
        {
            return Ok(await _service.MonthlyDataCloseExecute(param));
        }

        [HttpGet("GetListFactoryAdd")]
        public async Task<IActionResult> GetListFactoryAdd(string language)
        {
            return Ok(await _service.Queryt_Factory_AddList(userName, language));
        }

        [HttpGet("QueryDepartmentList")]
        public async Task<IActionResult> QueryDepartmentList(string factory, string language)
        {
            return Ok(await _service.Query_DropDown_List(factory, language));
        }
    }
}