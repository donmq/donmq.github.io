using API._Services.Interfaces.SalaryReport;
using API.DTOs.SalaryReport;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryReport
{
    public class C_7_2_6_MonthlyNonTransferSalaryPaymentReport : APIController
    {
        private readonly I_7_2_6_MonthlyNonTransferSalaryPaymentReport _service;

        public C_7_2_6_MonthlyNonTransferSalaryPaymentReport(I_7_2_6_MonthlyNonTransferSalaryPaymentReport service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(userName, language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }      

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _service.GetListDepartment(factory, language);
            return Ok(result);
        }

        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData([FromQuery] D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam param)
        {
            return Ok(await _service.SearchData(param));
        }

        [HttpGet("DownloadPdf")]
        public async Task<IActionResult> DownloadPdf([FromQuery] D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam param)
        {
            return Ok(await _service.DownloadFilePdf(param, userName));
        }


    }
}