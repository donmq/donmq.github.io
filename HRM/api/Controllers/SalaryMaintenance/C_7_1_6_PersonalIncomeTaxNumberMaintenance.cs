using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_6_PersonalIncomeTaxNumberMaintenanceController : APIController
    {
        private readonly I_7_1_6_PersonalIncomeTaxNumberMaintenance _service;

        public C_7_1_6_PersonalIncomeTaxNumberMaintenanceController(I_7_1_6_PersonalIncomeTaxNumberMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }
        
        [HttpGet("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate(string factory, string employeeID, string year)
        {
            return Ok(await _service.CheckDuplicate(factory, employeeID, year));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] PersonalIncomeTaxNumberMaintenanceParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] PersonalIncomeTaxNumberMaintenanceParam param, bool isTemplate)
        {
            return Ok(await _service.DownloadExcel(param, userName, isTemplate));
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            return Ok(await _service.UploadData(file, userName));
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] D_7_6_PersonalIncomeTaxNumberMaintenanceDto data)
        {
            var result = await _service.AddNew(data, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] D_7_6_PersonalIncomeTaxNumberMaintenanceDto data)
        {
            var result = await _service.Edit(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] D_7_6_PersonalIncomeTaxNumberMaintenanceDto data)
        {
            var result = await _service.Delete(data, userName);
            return Ok(result);
        }
    }
}