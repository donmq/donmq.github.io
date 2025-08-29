using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_27_LoanedMonthlyAttendanceDataMaintenance : APIController
    {
        private readonly I_5_1_27_LoanedMonthlyAttendanceDataMaintenance _service;

        public C_5_1_27_LoanedMonthlyAttendanceDataMaintenance(I_5_1_27_LoanedMonthlyAttendanceDataMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetEmployeeData")]
        public async Task<IActionResult> GetEmployeeData(string factory, string att_Month, string employeeID, string language)
        {
            return Ok(await _service.GetEmployeeData(factory, att_Month, employeeID, language));
        }

        [HttpGet("GetEmployeeID")]
        public async Task<IActionResult> GetEmployeeID(string factory)
        {
            return Ok(await _service.GetEmployeeID(factory));
        }

        [HttpGet("GetDataDetail")]
        public async Task<IActionResult> GetDataDetail([FromQuery] LoanedMonthlyAttendanceDataMaintenanceDto param)
        {
            return Ok(await _service.GetDataDetail(param));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] LoanedMonthlyAttendanceDataMaintenanceParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] LoanedMonthlyAttendanceDataMaintenanceParam param)
        {
            return Ok(await _service.DownloadExcel(param, userName));
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] LoanedMonthlyAttendanceDataMaintenanceDto data)
        {
            var result = await _service.AddNew(data, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] LoanedMonthlyAttendanceDataMaintenanceDto data)
        {
            var result = await _service.Edit(data, userName);
            return Ok(result);
        }
    }
}