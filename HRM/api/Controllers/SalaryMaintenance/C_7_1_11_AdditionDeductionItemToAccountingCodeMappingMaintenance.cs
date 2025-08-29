using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenanceController : APIController
    {
        private readonly I_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance _services;

        public C_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenanceController(I_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] AdditionDeductionItemToAccountingCodeMappingMaintenanceParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto)
        {
            var result = await _services.Create(dto, userName);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto)
        {
            var result = await _services.Update(dto, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto)
        {
            var result = await _services.Delete(dto);
            return Ok(result);
        }

        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser([FromQuery] string language)
        {
            var result = await _services.GetListFactoryByUser(userName, language);
            return Ok(result);
        }

        [HttpGet("GetListAdditionsAndDeductionsItem")]
        public async Task<IActionResult> GetListAdditionsAndDeductionsItem([FromQuery] string language)
        {
            var result = await _services.GetListAdditionsAndDeductionsItem(language);
            return Ok(result);
        }
    }
}