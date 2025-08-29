using API._Services.Interfaces.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
    public class C_6_2_1_MonthlyCompulsoryInsuranceDetailedReport : APIController
    {
        private readonly I_6_2_1_MonthlyCompulsoryInsuranceDetailedReport _service;
        public C_6_2_1_MonthlyCompulsoryInsuranceDetailedReport(I_6_2_1_MonthlyCompulsoryInsuranceDetailedReport service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpGet("GetListInsuranceType")]
        public async Task<IActionResult> GetListInsuranceType(string language)
        {
            return Ok(await _service.GetListInsuranceType(language));
        }

        [HttpGet("GetListPermissionGroupByFactory")]
        public async Task<IActionResult> GetListPermissionGroupByFactory([FromQuery] string factory, [FromQuery] string language)
        {
            var result = await _service.GetListPermissionGroupByFactory(factory, language);
            return Ok(result);
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments([FromQuery] string factory, [FromQuery] string language)
           => Ok(await _service.GetDepartments(factory, language));

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Download([FromQuery] D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("GetTotalRows")]
        public async Task<IActionResult> GetTotalRows([FromQuery] D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param)
        {
            var result = await _service.GetCountRecords(param);
            return Ok(result);
        }

    }
}