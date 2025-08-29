using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees : APIController
    {
        private readonly I_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees _service;

        public C_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees(I_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ResignedEmployeeDetail data)
            => Ok(await _service.Add(data, userName));

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] ResignedEmployeeDetail data)
            => Ok(await _service.Edit(data, userName));

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _service.GetListFactory(language));

        [HttpGet("GetListFactoryAdd")]
        public async Task<IActionResult> GetListFactoryAdd([FromQuery] string language)
            => Ok(await _service.GetListFactoryAdd(userName, language));

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _service.GetListDepartment(factory, language));

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string language)
            => Ok(await _service.GetListPermissionGroup(language));

        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType([FromQuery] string language)
            => Ok(await _service.GetListSalaryType(language));

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] ResignedEmployeeParam param)
            => Ok(await _service.GetDataPagination(pagination, param));

        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] ResignedEmployeeParam param)
            => Ok(await _service.Query(param));

        [HttpGet("GetEmpInfo")]
        public async Task<IActionResult> GetEmpInfo([FromQuery] ResignedEmployeeParam param)
            => Ok(await _service.GetEmpInfo(param));

        [HttpGet("GetResignedDetail")]
        public async Task<IActionResult> GetResignedDetail([FromQuery] ResignedEmployeeDetailParam param)
        {
            var (Leaves, Allowances) = await _service.GetResignedDetail(param);
            return Ok(new { leaves = Leaves, allowances = Allowances });
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParam pagination, [FromQuery] ResignedEmployeeParam param)
        {
            param.PrintBy = userName;
            return Ok(await _service.ExportExcel(pagination, param));
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