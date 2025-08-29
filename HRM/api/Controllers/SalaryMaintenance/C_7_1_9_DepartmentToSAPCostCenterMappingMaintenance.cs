
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_9_DepartmentToSAPCostCenterMappingMaintenanceController : APIController
    {
        private readonly I_7_1_9_DepartmentToSAPCostCenterMappingMaintenance _service;
        public C_7_1_9_DepartmentToSAPCostCenterMappingMaintenanceController(I_7_1_9_DepartmentToSAPCostCenterMappingMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] D_7_9_Sal_Dept_SAPCostCenter_MappingParam param)
        {
            var data = await _service.GetDataPagination(pagination, param);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data)
        {
            var result = await _service.Create(data, userName);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data)
        {
            var result = await _service.Update(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] D_7_9_Sal_Dept_SAPCostCenter_MappingDTO data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _service.UploadExcel(file, roleList, userName);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] D_7_9_Sal_Dept_SAPCostCenter_MappingParam param)
        {
            var result = await _service.DownloadExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("DownloadTemplate")]
        public async Task<ActionResult> DownloadTemplate()
        {
            var result = await _service.DownloadTemplate();
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string lang)
        {
            return Ok(await _service.GetListFactory(roleList, lang));
        }

        [HttpGet("GetListCostCenter")]
        public async Task<IActionResult> GetListCostCenter([FromQuery] D_7_9_Sal_Dept_SAPCostCenter_MappingParam param)
        {
            return Ok(await _service.GetListCostCenter(param));
        }
        
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _service.GetListDepartment(factory, language);
            return Ok(result);
        }

        [HttpGet("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate(string factory, string year, string department )
        {
            return Ok(await _service.CheckDuplicate(factory, year, department));
        }
    }
}