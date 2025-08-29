using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_14_ContractTypeSetup : APIController
    {
        private readonly I_4_1_14_ContractTypeSetup _services;
        public C_4_1_14_ContractTypeSetup(I_4_1_14_ContractTypeSetup services)
        {
            _services = services;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] ContractTypeSetupParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetDataDetail")]
        public async Task<IActionResult> GetDataDetail([FromQuery] ContractTypeSetupParam param)
        {
            var result = await _services.GetDataDetail(param);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ContractTypeSetupDto data)
        {
            var result = await _services.Add(data, userName);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ContractTypeSetupDto data)
        {
            var result = await _services.Edit(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] ContractTypeSetupDto data)
        {
            var result = await _services.Delete(data);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] ContractTypeSetupParam param)
        {
            var result = await _services.DownloadExcel(param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string language)
        {
            return Ok(await _services.GetListFactory(division, language));
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _services.GetListDivision(language));
        }

        [HttpGet("GetListContractType")]
        public async Task<IActionResult> GetListContractType(string division, string factory, string language)
        {
            return Ok(await _services.GetListContractType(division, factory, language));
        }

        [HttpGet("GetListScheduleFrequency")]
        public async Task<IActionResult> GetListScheduleFrequency(string language)
        {
            return Ok(await _services.GetListScheduleFrequency(language));
        }

        [HttpGet("GetListAlertRule")]
        public async Task<IActionResult> GetListAlertRule(string language)
        {
            return Ok(await _services.GetListAlertRule(language));
        }
    }
}