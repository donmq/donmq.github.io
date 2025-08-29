using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_16_SalaryMasterFileController: APIController
    {
        private readonly I_7_1_16_SalaryMasterFile _services;

        public C_7_1_16_SalaryMasterFileController(I_7_1_16_SalaryMasterFile services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] SalaryMasterFile_Param param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _services.GetFactorys(userName, language));

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _services.GetDepartments(factory, language));

        [HttpGet("GetPositionTitles")]
        public async Task<IActionResult> GetPositionTitles([FromQuery] string language)
            => Ok(await _services.GetPositionTitles(language));

        [HttpGet("GetSalaryTypes")]
        public async Task<IActionResult> GetSalaryTypes([FromQuery] string language)
            => Ok(await _services.GetSalaryTypes(language));

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string language)
            => Ok(await _services.GetListPermissionGroup(language));

        [HttpGet("GetDataQueryPage")]
        public async Task<IActionResult> GetDataQueryPage([FromQuery] PaginationParam pagination, [FromQuery] string factory, [FromQuery] string employee_ID, [FromQuery] string language)
            => Ok(await _services.GetDataQueryPage(pagination, factory, employee_ID, language));

        [HttpGet("GetTechnicalTypes")]
        public async Task<IActionResult> GetTechnicalTypes([FromQuery] string language)
            => Ok(await _services.GetTechnicalTypes(language));

        [HttpGet("GetExpertiseCategorys")]
        public async Task<IActionResult> GetExpertiseCategorys([FromQuery] string language)
            => Ok(await _services.GetExpertiseCategorys(language));

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] SalaryMasterFile_Param param)
        {
            var result = await _services.DownloadFileExcel(param, userName);
            return Ok(result);
        }

    }
}