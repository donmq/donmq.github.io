using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_15_AttendanceAbnormalityDataMaintenance : APIController
    {
        private readonly I_5_1_15_AttendanceAbnormalityDataMaintenance _service;

        public C_5_1_15_AttendanceAbnormalityDataMaintenance(I_5_1_15_AttendanceAbnormalityDataMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser(string language)
        {
            return Ok(await _service.GetListFactoryByUser(language, userName));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetListWorkShiftType")]
        public async Task<IActionResult> GetListWorkShiftType(string language)
        {
            return Ok(await _service.GetListWorkShiftType(language));
        }

        [HttpGet("GetListAttendance")]
        public async Task<IActionResult> GetListAttendance(string language)
        {
            return Ok(await _service.GetListAttendance(language));
        }

        [HttpGet("GetListUpdateReason")]
        public async Task<IActionResult> GetListUpdateReason(string language)
        {
            return Ok(await _service.GetListUpdateReason(language));
        }

        [HttpGet("GetListHoliday")]
        public async Task<IActionResult> GetListHoliday(string language)
        {
            return Ok(await _service.GetListHoliday(language));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] AttendanceAbnormalityDataMaintenanceParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] HRMS_Att_Temp_RecordDto data)
        {
            var result = await _service.AddNew(data, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] HRMS_Att_Temp_RecordDto data)
        {
            var result = await _service.Edit(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Att_Temp_RecordDto data)
        {
            var result = await _service.Delete(data, userName);
            return Ok(result);
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] AttendanceAbnormalityDataMaintenanceParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }
    }
}