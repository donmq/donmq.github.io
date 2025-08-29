using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_14_IncomeTaxFreeSettingController : APIController
    {
        private readonly I_7_1_14_IncomeTaxFreeSetting _services;

        public C_7_1_14_IncomeTaxFreeSettingController(I_7_1_14_IncomeTaxFreeSetting services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] IncomeTaxFreeSetting_MainParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] IncomeTaxFreeSetting_SubParam param)
        {
            var result = await _services.GetDetail(param);
            return Ok(result);
        }

        [HttpGet("IsDuplicatedData")]
        public async Task<IActionResult> IsDuplicatedData([FromQuery] IncomeTaxFreeSetting_SubParam param)
        {
            var result = await _services.IsDuplicatedData(param, userName);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] IncomeTaxFreeSetting_Form data)
        {
            var result = await _services.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] IncomeTaxFreeSetting_Form data)
        {
            var result = await _services.Update(data);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] IncomeTaxFreeSetting_MainData data)
        {
            var result = await _services.Delete(data);
            return Ok(result);
        }

        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser(string language)
        {
            var result = await _services.GetListFactoryByUser(language, roleList);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string Language)
        {
            var result = await _services.GetListFactory(Language);
            return Ok(result);
        }

        [HttpGet("GetListType")]
        public async Task<IActionResult> GetListType([FromQuery] string Language)
        {
            var result = await _services.GetListType(Language);
            return Ok(result);
        }

        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType([FromQuery] string Language)
        {
            var result = await _services.GetListSalaryType(Language);
            return Ok(result);
        }
    }
}