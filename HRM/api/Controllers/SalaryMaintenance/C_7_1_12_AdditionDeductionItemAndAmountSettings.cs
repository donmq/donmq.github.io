using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_12_AdditionDeductionItemAndAmountSettingsController : APIController
    {
        private readonly I_7_1_12_AdditionDeductionItemAndAmountSettings _services;

        public C_7_1_12_AdditionDeductionItemAndAmountSettingsController(I_7_1_12_AdditionDeductionItemAndAmountSettings services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] AdditionDeductionItemAndAmountSettingsParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] AdditionDeductionItemAndAmountSettings_SubParam param)
        {
            var result = await _services.GetDetail(param);
            return Ok(result);
        }

        [HttpGet("CheckData")]
        public async Task<IActionResult> CheckData([FromQuery] AdditionDeductionItemAndAmountSettings_SubParam param)
        {
            var result = await _services.CheckData(param, userName);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AdditionDeductionItemAndAmountSettings_Form dto)
        {
            var result = await _services.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AdditionDeductionItemAndAmountSettings_Form dto)
        {
            var result = await _services.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] AdditionDeductionItemAndAmountSettingsDto dto)
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

        [HttpGet("GetListPermissionGroupByFactory")]
        public async Task<IActionResult> GetListPermissionGroupByFactory([FromQuery] string factory, [FromQuery] string language)
        {
            var result = await _services.GetListPermissionGroupByFactory(factory, language);
            return Ok(result);
        }

        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType([FromQuery] string language)
        {
            var result = await _services.GetListSalaryType(language);
            return Ok(result);
        }

        [HttpGet("GetListAdditionsAndDeductionsType")]
        public async Task<IActionResult> GetListAdditionsAndDeductionsType([FromQuery] string language)
        {
            var result = await _services.GetListAdditionsAndDeductionsType(language);
            return Ok(result);
        }

        [HttpGet("GetListAdditionsAndDeductionsItem")]
        public async Task<IActionResult> GetListAdditionsAndDeductionsItem([FromQuery] string language)
        {
            var result = await _services.GetListAdditionsAndDeductionsItem(language);
            return Ok(result);
        }
        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] AdditionDeductionItemAndAmountSettingsParam param)
        {
            var result = await _services.DownloadFileExcel(param, userName);
            return Ok(result);
        }
    }
}