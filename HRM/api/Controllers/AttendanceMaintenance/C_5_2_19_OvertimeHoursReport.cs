using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_2_19_OvertimeHoursReport: APIController
    {
        private readonly I_5_2_19_OvertimeHoursReport _service;
        public C_5_2_19_OvertimeHoursReport(I_5_2_19_OvertimeHoursReport service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] OvertimeHoursReportParam param)
        {
            var result = await _service.GetData(param,roleList);
            return Ok(result);
        }

        [HttpGet("GetListFactoryAdd")]
        public async Task<IActionResult> GetListFactoryAdd(string language)
        {
            var result = await _service.Query_Factory_AddList(userName, language);
            return Ok(result);
        }

        [HttpGet("GetListWorkShiftType")]
        public async Task<IActionResult> GetListWorkShiftType(string language)
        {
            var result = await _service.GetListWorkShiftType(language);
            return Ok(result);
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _service.Query_DropDown_List(factory, language);
            return Ok(result);
        }
        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string language,string factory)
        {
            var result = await _service.GetListPermissionGroup(language, factory);
            return Ok(result);
        }
        [HttpGet("Export")]
        public async Task<IActionResult> Export([FromQuery]OvertimeHoursReportParam param)
        {
            var result = await _service.Export(param,roleList,userName);
            return Ok(result);
        }
    }
}