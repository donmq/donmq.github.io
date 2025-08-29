using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_3_LeaveSalaryCalculationMaintenanceController : APIController
    {
        private readonly I_7_1_3_LeaveSalaryCalculationMaintenance _service;
        public C_7_1_3_LeaveSalaryCalculationMaintenanceController(I_7_1_3_LeaveSalaryCalculationMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] LeaveSalaryCalculationMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListLeaveCode")]
        public async Task<IActionResult> GetListLeaveCode(string language)
        {
            return Ok(await _service.GetListLeaveCode(language));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LeaveSalaryCalculationMaintenanceDTO dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LeaveSalaryCalculationMaintenanceDTO data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] LeaveSalaryCalculationMaintenanceDTO data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }

    }
}