using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_13_IncomeTaxBracketSettingController : APIController
    {
        private readonly I_7_1_13_IncomeTaxBracketSetting _services;

        public C_7_1_13_IncomeTaxBracketSettingController(I_7_1_13_IncomeTaxBracketSetting services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] IncomeTaxBracketSettingParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] IncomeTaxBracketSettingDto dto)
        {
            var result = await _services.GetDetail(dto);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] IncomeTaxBracketSettingDto dto)
        {
            var result = await _services.Create(dto, userName);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] IncomeTaxBracketSettingDto dto)
        {
            var result = await _services.Update(dto, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] IncomeTaxBracketSettingDto dto)
        {
            var result = await _services.Delete(dto);
            return Ok(result);
        }

        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality([FromQuery] string Language)
        {
            var result = await _services.GetListNationality(Language);
            return Ok(result);
        }

        [HttpGet("GetListTaxCode")]
        public async Task<IActionResult> GetListTaxCode([FromQuery] string Language)
        {
            var result = await _services.GetListTaxCode(Language);
            return Ok(result);
        }
        
        [HttpGet("IsDuplicatedData")]
        public async Task<IActionResult> IsDuplicatedData(string Nation, string Tax_Code, int Tax_Level, string Effective_Month)
        {
            return Ok(await _services.IsDuplicatedData(Nation, Tax_Code, Tax_Level, Effective_Month));
        }
    }
}