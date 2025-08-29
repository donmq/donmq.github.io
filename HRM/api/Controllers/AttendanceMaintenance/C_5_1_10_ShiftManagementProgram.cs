using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_10_ShiftManagementProgram : APIController
    {
        private readonly I_5_1_10_ShiftManagementProgram _service;
        public C_5_1_10_ShiftManagementProgram(I_5_1_10_ShiftManagementProgram service)
        {
            _service = service;
        }
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList([FromQuery] ShiftManagementProgram_Param param)
        {
            var result = await _service.GetDropDownList(param, roleList);
            return Ok(result);
        }
        [HttpGet("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList([FromQuery] ShiftManagementProgram_Param param)
        {
            var result = await _service.GetDepartmentList(param);
            return Ok(result);
        }
        [HttpGet("GetWorkShiftTypeDepartmentList")]
        public async Task<IActionResult> GetWorkShiftTypeDepartmentList([FromQuery] ShiftManagementProgram_Param param)
        {
            var result = await _service.GetWorkShiftTypeDepartmentList(param);
            return Ok(result);
        }
        [HttpGet("IsExistedData")]
        public async Task<IActionResult> IsExistedData([FromQuery] ShiftManagementProgram_Param param)
        {
            return Ok(await _service.IsExistedData(param));
        }
        [HttpGet("GetSearchDetail")]
        public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] ShiftManagementProgram_Param filter)
        {
            var result = await _service.GetSearchDetail(param, filter);
            return Ok(result);
        }
        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList([FromQuery] ShiftManagementProgram_Param param)
        {
            var result = await _service.GetEmployeeList(param);
            return Ok(result);
        }
        [HttpGet("GetEmployeeDetail")]
        public async Task<IActionResult> GetEmployeeDetail([FromQuery] ShiftManagementProgram_Param param)
        {
            var result = await _service.GetEmployeeDetail(param);
            return Ok(result);
        }
        [HttpPost("PostDataEmployee")]
        public async Task<IActionResult> PostDataEmployee([FromBody] ShiftManagementProgram_Main data)
        {
            var result = await _service.PostDataEmployee(data);
            return Ok(result);
        }
        [HttpPost("PostDataDepartment")]
        public async Task<IActionResult> PostDataDepartment([FromBody] List<ShiftManagementProgram_Main> data)
        {
            var result = await _service.PostDataDepartment(data);
            return Ok(result);
        }
        [HttpPut("PutDataEmployee")]
        public async Task<IActionResult> PutDataEmployee([FromBody] ShiftManagementProgram_Update data)
        {
            var result = await _service.PutDataEmployee(data);
            return Ok(result);
        }
        [HttpDelete("BatchDelete")]
        public async Task<IActionResult> BatchDelete([FromBody] List<ShiftManagementProgram_Main> data)
        {
            var result = await _service.BatchDelete(data);
            return Ok(result);
        }
    }
}