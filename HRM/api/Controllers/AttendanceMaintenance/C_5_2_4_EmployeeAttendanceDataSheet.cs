using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_4_EmployeeAttendanceDataSheet : APIController
    {
        private readonly I_5_2_4_EmployeeAttendanceDataSheet _service;

        public C_5_2_4_EmployeeAttendanceDataSheet(I_5_2_4_EmployeeAttendanceDataSheet service)
        {
            _service = service;
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] EmployeeAttendanceDataSheetParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("GetCountRecords")]
        public async Task<IActionResult> GetCountRecords([FromQuery] EmployeeAttendanceDataSheetParam param)
        {
            var result = await _service.GetCountRecords(param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(roleList, language));
        }

        [HttpGet("GetListWorkShiftType")]
        public async Task<IActionResult> GetListWorkShiftType(string language)
        {
            return Ok(await _service.GetListWorkShiftType(language));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }
    }
}