using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees : APIController
    {
        private readonly I_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees _service;
        public C_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees(I_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees service)
        {
            _service = service;
        }

        [HttpGet("Query")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] MaintenanceActiveEmployeesParam param)
        {
            return Ok(await _service.GetPagination(pagination, param));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] MaintenanceActiveEmployeesDetail data)
        {
            return Ok(await _service.Add(data, userName));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] MaintenanceActiveEmployeesDetail data)
        {
            return Ok(await _service.Edit(data, userName));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] MaintenanceActiveEmployeesDetailParam param)
        {
            return Ok(await _service.Detail(param));
        }

        [HttpGet("GetEmpInfo")]
        public async Task<IActionResult> GetEmpInfo([FromQuery] ActiveEmployeeParam param)
        {
            return Ok(await _service.GetEmpInfo(param));
        }

        [HttpGet("GetLeaveAllowance")]
        public async Task<IActionResult> GetLeaveAllowance([FromQuery] MaintenanceActiveEmployeesDetailParam param)
        {
            var result = await _service.GetLeaveAllowance(param);
            return Ok(new { Leave = result.Item1, Allowance = result.Item2 });
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] MaintenanceActiveEmployeesParam param)
        {
            return Ok(await _service.Download(param, userName));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string language)
        {
            return Ok(await _service.GetListPermissionGroup(language));
        }

        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType(string language)
        {
            return Ok(await _service.GetListSalaryType(language));
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
        [HttpGet("GetEmployeeIDByFactorys")]
        public async Task<IActionResult> GetEmployeeIDByFactorys(string factory)
        {
            var result = await _service.GetEmployeeIDByFactorys(factory);
            return Ok(result);
        }
        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser(string language)
        {
            return Ok(await _service.GetListFactoryByUser(language, userName));
        }
    }
}