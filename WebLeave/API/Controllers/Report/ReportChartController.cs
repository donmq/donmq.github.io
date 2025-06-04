using API._Services.Interfaces.Report;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Report
{
    public class ReportChartController : ApiController
    {
        private readonly IReportChartService _service;
        public ReportChartController(IReportChartService service) {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData() {
            var data = await _service.GetDataChart();
            return Ok(data);
        }

        [HttpGet("GetDataChartInArea")]
        public async Task<IActionResult> GetDataChartInArea(int areaId) {
            var data = await _service.GetDataChartInArea(areaId);
            return Ok(data);
        }
    }
}