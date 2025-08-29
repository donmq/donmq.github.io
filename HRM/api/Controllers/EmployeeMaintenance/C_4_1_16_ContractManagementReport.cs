using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_16_ContractManagementReport : APIController
    {
        private readonly I_4_1_16_ContractManagementReport _service;
        public C_4_1_16_ContractManagementReport(I_4_1_16_ContractManagementReport service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] ContractManagementReportParam param)
        {
            var data = await _service.GetDataPagination(pagination, param);
            return Ok(data);
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] ContractManagementReportParam param)
        {
            var result = await _service.DownloadExcel(param);
            return Ok(result);
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string lang)
        {
            return Ok(await _service.GetListDivision(lang));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string lang)
        {
            return Ok(await _service.GetListFactory(division, lang));
        }

        [HttpGet("GetListContractType")]
        public async Task<IActionResult> GetListContractType(string division, string factory, string lang)
        {
            return Ok(await _service.GetListContractType(division, factory, lang));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string lang)
        {
            return Ok(await _service.GetListDepartment(division, factory, lang));
        }

    }
}