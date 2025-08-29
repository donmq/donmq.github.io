using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_16_OvertimeTemporaryRecordMaintenance : APIController
    {
        private readonly I_5_1_16_OvertimeTemporaryRecordMaintenance _service;
        public C_5_1_16_OvertimeTemporaryRecordMaintenance(I_5_1_16_OvertimeTemporaryRecordMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Att_Overtime_TempParam param)
        {
            var result = await _service.GetData(pagination, param);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Att_Overtime_TempDto data)
        {
            return Ok(await _service.Create(data, userName));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Att_Overtime_TempDto data)
        {
            return Ok(await _service.Update(data, userName));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Att_Overtime_TempDto data)
        {
            return Ok(await _service.Delete(data));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string lang)
        {
            return Ok(await _service.GetListFactory(lang, roleList));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string lang)
        {
            return Ok(await _service.GetListDepartment(factory, lang));
        }

        [HttpGet("GetListWorkShiftType")]
        public async Task<IActionResult> GetListWorkShiftType(string lang)
        {
            return Ok(await _service.GetListWorkShiftType(lang));
        }

        [HttpGet("GetListHoliday")]
        public async Task<IActionResult> GetListHoliday(string lang)
        {
            return Ok(await _service.GetListHoliday(lang));
        }

        [HttpGet("GetClockInOutByTempRecord")]
        public async Task<IActionResult> GetClockInOutByTempRecord([FromQuery] OvertimeTempPersonalParam param)
        {
            return Ok(await _service.GetClockInOutByTempRecord(param));
        }

        [HttpGet("GetShiftTimeByWorkShift")]
        public async Task<IActionResult> GetShiftTimeByWorkShift(string factory, string workShiftType , string date)
        {
            return Ok(await _service.GetShiftTimeByWorkShift(factory, workShiftType,date));
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] HRMS_Att_Overtime_TempParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

    }
}