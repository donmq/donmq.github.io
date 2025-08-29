using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_1_NewResignedEmployeeDataPrinting : APIController
    {
        private readonly I_5_2_1_NewResignedEmployeeDataPrinting _service;

        public C_5_2_1_NewResignedEmployeeDataPrinting(I_5_2_1_NewResignedEmployeeDataPrinting service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language, factory));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] NewResignedEmployeeDataPrintingParam param)
        {
            return Ok(await _service.GetData(param));
        }

        [HttpGet("GetTotal")]
        public async Task<IActionResult> GetTotal([FromQuery] NewResignedEmployeeDataPrintingParam param)
        {
            return Ok(await _service.GetTotal(param));
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] NewResignedEmployeeDataPrintingParam param)
        {
            return Ok(await _service.DownloadExcel(param, userName));
        }
    }
}