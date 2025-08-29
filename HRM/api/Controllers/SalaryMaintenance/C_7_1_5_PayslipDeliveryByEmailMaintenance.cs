using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_5_PayslipDeliveryByEmailMaintenanceController : APIController
    {
        private readonly I_7_1_5_PayslipDeliveryByEmailMaintenance _service;

        public C_7_1_5_PayslipDeliveryByEmailMaintenanceController(I_7_1_5_PayslipDeliveryByEmailMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }
        
        [HttpGet("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate(string factory, string employeeID)
        {
            return Ok(await _service.CheckDuplicate(factory, employeeID));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] PayslipDeliveryByEmailMaintenanceParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] PayslipDeliveryByEmailMaintenanceParam param, bool isTemplate)
        {
            return Ok(await _service.DownloadExcel(param, userName, isTemplate));
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            return Ok(await _service.UploadData(file, userName));
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] D_7_5_PayslipDeliveryByEmailMaintenanceDto data)
        {
            var result = await _service.AddNew(data, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] D_7_5_PayslipDeliveryByEmailMaintenanceDto data)
        {
            var result = await _service.Edit(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] D_7_5_PayslipDeliveryByEmailMaintenanceDto data)
        {
            var result = await _service.Delete(data, userName);
            return Ok(result);
        }
    }
}