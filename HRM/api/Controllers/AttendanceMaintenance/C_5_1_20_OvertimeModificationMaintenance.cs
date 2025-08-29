using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_20_OvertimeModificationMaintenance : APIController
    {
        private readonly I_5_1_20_OvertimeModificationMaintenance _service;
        public C_5_1_20_OvertimeModificationMaintenance(I_5_1_20_OvertimeModificationMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] OvertimeModificationMaintenanceParam param)
        {
            var result = await _service.GetData(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, userName);
            return Ok(result);
        }

        [HttpGet("GetWorkShiftType")]
        public async Task<IActionResult> GetWorkShiftType([FromQuery] OvertimeModificationMaintenanceParam param)
        {
            var result = await _service.GetWorkShiftType(param);
            return Ok(result);
        }

        [HttpGet("GetListHoliday")]
        public async Task<IActionResult> GetListHoliday(string language)
        {
            var result = await _service.GetListHoliday(language);
            return Ok(result);
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _service.GetListDepartment(factory, language);
            return Ok(result);
        }

        [HttpGet("GetWorkShiftTypeTime")]
        public async Task<IActionResult> GetWorkShiftTypeTime(string work_Shift_Type, string date, string factory)
        {
            var result = await _service.GetWorkShiftTypeTime(work_Shift_Type, date, factory);
            return Ok(result);
        }

        [HttpGet("GetClockInTimeAndClockOutTimeByEmpIdAndDate")]
        public async Task<IActionResult> GetClockInTimeAndClockOutTimeByEmpIdAndDate(string employee_ID, string date)
        {
            var result = await _service.GetClockInTimeAndClockOutTimeByEmpIdAndDate(employee_ID, date);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OvertimeModificationMaintenanceDto model)
        {
            var result = await _service.Create(model, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] OvertimeModificationMaintenanceDto model)
        {
            var result = await _service.Edit(model, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] OvertimeModificationMaintenanceDto model)
        {
            var result = await _service.Delete(model, userName);
            return Ok(result);
        }
    }
}