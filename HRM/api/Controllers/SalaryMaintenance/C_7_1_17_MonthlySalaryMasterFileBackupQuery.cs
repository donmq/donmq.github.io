using API._Services.Interfaces.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_17_MonthlySalaryMasterFileBackupQueryController : APIController
    {
        private readonly I_7_1_17_MonthlySalaryMasterFileBackupQuery _service;

        public C_7_1_17_MonthlySalaryMasterFileBackupQueryController(I_7_1_17_MonthlySalaryMasterFileBackupQuery service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] MonthlySalaryMasterFileBackupQueryParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpGet("GetSalaryDetails")]
        public async Task<IActionResult> GetSalaryDetails([FromQuery] PaginationParam pagination, string probation, string factory, string employeeID, string language, string yearMonth)
        {
            return Ok(await _service.GetSalaryDetails(pagination, probation, factory, employeeID, language, yearMonth));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetListPositionTitle")]
        public async Task<IActionResult> GetListPositionTitle(string language)
        {
            return Ok(await _service.GetListPositionTitle(language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string language)
        {
            return Ok(await _service.GetListPermissionGroup(language));
        }

        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType(string language)
        {
            return Ok(await _service.GetListSalaryType(language));
        }

        [HttpGet("GetListTechnicalType")]
        public async Task<IActionResult> GetListTechnicalType(string language)
        {
            return Ok(await _service.GetListTechnicalType(language));
        }

        [HttpGet("GetListExpertiseCategory")]
        public async Task<IActionResult> GetListExpertiseCategory(string language)
        {
            return Ok(await _service.GetListExpertiseCategory(language));
        }

        [HttpPost("Execute")]
        public async Task<IActionResult> Execute([FromBody] MonthlySalaryMasterFileBackupQueryParam param)
        {
            return Ok(await _service.Execute(param, userName));
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] MonthlySalaryMasterFileBackupQueryParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }
    }
}