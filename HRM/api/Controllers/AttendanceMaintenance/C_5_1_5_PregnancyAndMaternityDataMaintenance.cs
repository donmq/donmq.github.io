using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_5_PregnancyAndMaternityDataMaintenance : APIController
    {
        private readonly I_5_1_5_PregnancyAndMaternityDataMaintenance _service;

        public C_5_1_5_PregnancyAndMaternityDataMaintenance(I_5_1_5_PregnancyAndMaternityDataMaintenance service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] PregnancyMaternityDetail dto)
        {
            dto.Update_By = userName;
            return Ok(await _service.Add(dto));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] PregnancyMaternityDetail dto)
        {
            dto.Update_By = userName;
            return Ok(await _service.Edit(dto));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] PregnancyMaternityDetail dto)
            => Ok(await _service.Delete(dto));

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _service.GetListFactory(language, userName));
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _service.GetListDepartment(factory, language));

        [HttpGet("GetListWorkType")]
        public async Task<IActionResult> GetListWorkType([FromQuery] string language, bool isWorkShiftType)
            => Ok(await _service.GetListWorkType(language, isWorkShiftType));

        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] PaginationParam pagination, [FromQuery] PregnancyMaternityParam param)
            => Ok(await _service.Query(pagination, param));

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PregnancyMaternityParam param)
            => Ok(await _service.ExportExcel(param, userName));

        [HttpGet("GetSpecialRegularWorkType")]
        public async Task<IActionResult> GetSpecialRegularWorkType(string Factory, string Work_Type_Before)
        {
            return Ok(await _service.GetSpecialRegularWorkType(Factory, Work_Type_Before));
        }
    }
}