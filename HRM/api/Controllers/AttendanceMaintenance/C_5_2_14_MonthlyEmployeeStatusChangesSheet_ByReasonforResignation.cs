using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation : APIController
    {
        private readonly I_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation _services;
        public C_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation(I_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation services)
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

        [HttpGet("GetPermistionGroups")]
        public async Task<IActionResult> GetPermistionGroups(string factory, string language)
        {
            var result = await _services.GetPermistionGroups(factory, language);
            return Ok(result);
        }
        #endregion


        [HttpPost("GetTotalRecords")]
        public async Task<IActionResult> GetTotalRecords([FromBody] Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param)
        {
            var result = await _services.GetTotalRecords(param);
            return Ok(result);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export([FromBody] Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param)
        {
            param.PrintBy = userName;
            var result = await _services.Export(param);
            return Ok(result);
        }
    }
}