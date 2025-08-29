using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_4_BankAccountMaintenanceController: APIController
    {
         private readonly I_7_1_4_BankAccountMaintenance _service;
        public C_7_1_4_BankAccountMaintenanceController(I_7_1_4_BankAccountMaintenance service)
        {
            _service = service;
        }
         [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] BankAccountMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankAccountMaintenanceDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankAccountMaintenanceDto dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] BankAccountMaintenanceDto dto)
        {
            var result = await _service.Delete(dto);
            return Ok(result);
        }
        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] BankAccountMaintenanceParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpPost("UploadFileExcel")]
        public async Task<IActionResult> UploadFileExcel([FromForm] BankAccountMaintenanceUpload param)
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