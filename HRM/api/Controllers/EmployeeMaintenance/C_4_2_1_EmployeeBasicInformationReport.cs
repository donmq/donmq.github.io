using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{

    public class C_4_2_1_EmployeeBasicInformationReport : APIController
    {
        private readonly I_4_2_1_EmployeeBasicInformationReport _services;
        public C_4_2_1_EmployeeBasicInformationReport(I_4_2_1_EmployeeBasicInformationReport services)
        {
            _services = services;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] EmployeeBasicInformationReport_Param param)
        {
            var result = await _services.GetPagination(param, roleList);
            return Ok(result);
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export([FromQuery] EmployeeBasicInformationReport_Param param)
        {
            param.UserName = userName;
            var result = await _services.ExportExcel(param, roleList);
            return Ok(result);
        }

        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality(string Language)
        {
            return Ok(await _services.GetListNationality(Language));
        }


        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string Language)
        {
            return Ok(await _services.GetListDivision(Language));
        }


        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string language)
        {
            return Ok(await _services.GetListFactory(division, roleList, language));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string language)
        {
            return Ok(await _services.GetListDepartment(division, factory, language));
        }
        
        [HttpGet("GetListPositonGrade")]
        public async Task<IActionResult> GetListPositonGrade()
        {
            return Ok(await _services.GetListPositonGrade());
        }

        [HttpGet("GetListPermission")]
        public async Task<IActionResult> GetListPermission(string language)
        {
            return Ok(await _services.GetListPermission(language));
        }
    }
}