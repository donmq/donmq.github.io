using API._Services.Interfaces.Report;
using API.Dtos.Report.ReportShow;
using API.Helpers.Params.ReportShow;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Report
{
    public class ReportController : ApiController
    {
        private readonly IReportService _reportService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(
            IReportService reportService,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _reportService = reportService;
        }

        [HttpGet("ReportShowIndex")]
        public async Task<IActionResult> ReportShow([FromQuery] ReportShowParam param)
        {
            var result = await _reportService.ReportShow(param);
            return Ok(result);
        }

        [HttpGet("ReportGridDetail")]
        public async Task<IActionResult> ReportGridDetail([FromQuery] ReportGridDetailParam param)
        {
            var result = await _reportService.ReportGridDetail(param);
            return Ok(result);
        }

        [HttpGet("ReportDateDetail")]
        public async Task<IActionResult> ReportDateDetail([FromQuery] ReportDateDetailParam param)
        {
            var result = await _reportService.ReportDateDetail(param);
            return Ok(result);
        }

        [HttpPost("ExportDateDetail")]
        public async Task<IActionResult> ExportDateDetail([FromBody] ExportExcelDateDto model)
        {
            var result = await _reportService.ExportDateDetail(model);
            return Ok(result);
        }

        // Xuất dữ liệu ra excel ở dạng lưới
        [HttpPost("ExportGridDetail")]
        public async Task<IActionResult> ExportGridDetail([FromBody] ExportExcelGridDto model)
        {
           var result = await _reportService.ExportGridDetail(model);
            return Ok(result);
        }
    }
}