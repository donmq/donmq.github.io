using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_10_SalaryItemToAccountingCodeMappingMaintenanceController: APIController
    {
        private readonly I_7_1_10_SalaryItemToAccountingCodeMappingMaintenance _service;

        public C_7_1_10_SalaryItemToAccountingCodeMappingMaintenanceController(I_7_1_10_SalaryItemToAccountingCodeMappingMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] D_7_10_SalaryItemToAccountingCodeMappingMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetFactory")]
        public async Task<IActionResult> GetFactory(string language)
        {
            var result = await _service.GetFactory(userName, language);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto model)
        {
            var result = await _service.Create(model, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto model)
        {
            model.Update_By = userName;
            var result = await _service.Edit(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string factory, string Salary_Item, string DC_Code)
        {
            var result = await _service.Delete(factory, Salary_Item, DC_Code);
            return Ok(result);
        }

        [HttpGet("CheckDupplicate")]
        public async Task<IActionResult> CheckDupplicate(string factory, string Salary_Item, string DC_Code)
        {
            var result = await _service.CheckDupplicate(factory, Salary_Item, DC_Code);
            return Ok(result);
        }
    }
}