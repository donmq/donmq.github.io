using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class ReportCheckMachineSafetyController : ApiController
    {
        private readonly IReportCheckMachineSafetyService _reportcheckMachineSafetyService;

        public ReportCheckMachineSafetyController(IReportCheckMachineSafetyService reportCheckMachineSSafetyervice)
        {
            _reportcheckMachineSafetyService = reportCheckMachineSSafetyervice;
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] ReportCheckMachineSafetyParam param)
        {
            var data = await _reportcheckMachineSafetyService.ExportExcel(param);
            return Ok(data);
        }

    }
}