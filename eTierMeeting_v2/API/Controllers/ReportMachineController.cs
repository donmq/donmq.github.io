using Machine_API._Service.interfaces;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class ReportMachineController : ApiController
    {
        private readonly IMachineReportService _machineReport;

        public ReportMachineController(IMachineReportService machineReport)
        {
            _machineReport = machineReport;
        }

        [HttpPost("GetListReportMachine")]
        public async Task<IActionResult> GetListReportMachine(SearchMachineParams searchMachineParams)
        {
            var data = await _machineReport.GetListReportMachine(searchMachineParams);
            return Ok(data);
        }
    }
}