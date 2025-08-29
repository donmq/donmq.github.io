using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_8_EmployeeGroupSkillSettings : APIController
    {
        private readonly I_4_1_8_EmployeeGroupSkillSettings _service;
        public C_4_1_8_EmployeeGroupSkillSettings(I_4_1_8_EmployeeGroupSkillSettings service)
        {
            _service = service;
        }
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList([FromQuery] EmployeeGroupSkillSettings_Param param)
        {
            var result = await _service.GetDropDownList(param);
            return Ok(result);
        }
        [HttpGet("GetSearchDetail")]
        public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] EmployeeGroupSkillSettings_Param filter)
        {
            var result = await _service.GetSearchDetail(param, filter, roleList);
            return Ok(result);
        }
        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList([FromQuery] EmployeeGroupSkillSettings_Param param)
        {
            var result = await _service.GetEmployeeList(param);
            return Ok(result);
        }
        [HttpGet("CheckExistedData")]
        public async Task<IActionResult> CheckExistedData([FromQuery] EmployeeGroupSkillSettings_Param param)
        {
            var result = await _service.CheckExistedData(param);
            return Ok(result);
        }
        [HttpPost("PostData")]
        public async Task<IActionResult> PostData([FromBody] EmployeeGroupSkillSettings_Main data)
        {
            var result = await _service.PostData(data, userName);
            return Ok(result);
        }
        [HttpPut("PutData")]
        public async Task<IActionResult> PutData([FromBody] EmployeeGroupSkillSettings_Main data)
        {
            var result = await _service.PutData(data, userName);
            return Ok(result);
        }
        [HttpDelete("DeleteData")]
        public async Task<IActionResult> DeleteData([FromQuery] string division, string factory, string employee_Id)
        {
            var result = await _service.DeleteData( division, factory, employee_Id);
            return Ok(result);
        }
    }
}