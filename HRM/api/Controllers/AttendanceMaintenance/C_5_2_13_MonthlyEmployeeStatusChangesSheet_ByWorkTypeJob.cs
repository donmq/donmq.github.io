using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob : APIController
    {
        private readonly I_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob _services;
        public C_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob(I_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob services)
        {
            _services = services;
        }


        #region Get Dropdown

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetFactories(roleList, language);
            return Ok(result);
        }

        [HttpGet("GetLevels")]
        public async Task<IActionResult> GetLevels(string language)
        {
            var result = await _services.GetLevels(language);
            return Ok(result);
        }

        [HttpGet("GetPermistionGroups")]
        public async Task<IActionResult> GetPermistionGroups(string factory, string language)
        {
            var result = await _services.GetPermistionGroups(factory, language);
            return Ok(result);
        }

        [HttpGet("GetWorkTypeJobs")]
        public async Task<IActionResult> GetWorkTypeJobs(string language)
        {
            var result = await _services.GetWorkTypeJobs(language);
            return Ok(result);
        }
        #endregion


        [HttpPost("GetTotalRecords")]
        public async Task<IActionResult> GetTotalRecords([FromBody] Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Param param)
        {
            var result = await _services.GetTotalRecords(param);
            return Ok(result);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export([FromBody] Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Param param)
        {
            param.PrintBy = userName;
            var result = await _services.ExportExcel(param);
            return Ok(result);
        }
    }
}