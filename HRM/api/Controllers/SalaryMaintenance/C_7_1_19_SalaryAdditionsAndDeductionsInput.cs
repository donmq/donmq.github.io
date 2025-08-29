
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_19_SalaryAdditionsAndDeductionsInputController : APIController
    {
        private readonly I_7_1_19_SalaryAdditionsAndDeductionsInput _service;
        public C_7_1_19_SalaryAdditionsAndDeductionsInputController(I_7_1_19_SalaryAdditionsAndDeductionsInput service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] SalaryAdditionsAndDeductionsInput_Param param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language, factory));
        }

        [HttpGet("GetListAddDedType")]
        public async Task<IActionResult> GetListAddDedType(string language)
        {
            return Ok(await _service.GetListAddDedType(language));
        }

        [HttpGet("GetListAddDedItem")]
        public async Task<IActionResult> GetListAddDedItem(string language)
        {
            return Ok(await _service.GetListAddDedItem(language));
        }

        [HttpGet("GetListCurrency")]
        public async Task<IActionResult> GetListCurrency(string language)
        {
            return Ok(await _service.GetListCurrency(language));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SalaryAdditionsAndDeductionsInputDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SalaryAdditionsAndDeductionsInputDto dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] SalaryAdditionsAndDeductionsInputDto dto)
        {
            var result = await _service.Delete(dto);
            return Ok(result);
        }
        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] SalaryAdditionsAndDeductionsInput_Param param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpPost("UploadFileExcel")]
        public async Task<IActionResult> UploadFileExcel([FromForm] SalaryAdditionsAndDeductionsInput_Upload param)
        {
            return Ok(await _service.UploadFileExcel(param, roleList, userName));
        }

        [HttpGet("DownloadFileTemplate")]
        public async Task<IActionResult> DownloadFileTemplate()
        {
            var result = await _service.DownloadFileTemplate();
            return Ok(result);
        }
    }
}