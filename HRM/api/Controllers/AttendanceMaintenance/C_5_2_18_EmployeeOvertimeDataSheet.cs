
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_18_EmployeeOvertimeDataSheet : APIController
    {
        private readonly I_5_2_18_EmployeeOvertimeDataSheet _services;

        public C_5_2_18_EmployeeOvertimeDataSheet(I_5_2_18_EmployeeOvertimeDataSheet services)
        {
            _services = services;
        }

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetListFactory(language, userName);
            return Ok(result);
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] EmployeeOvertimeDataSheetParam param)
        {
            var result = await _services.GetPagination(param);
            return Ok(result);
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export([FromQuery] EmployeeOvertimeDataSheetParam param)
        {
            param.UserName = userName;
            var result = await _services.ExportExcel(param);
            return Ok(result);
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _services.GetListDepartment(factory, language);
            return Ok(result);
        }

    }
}