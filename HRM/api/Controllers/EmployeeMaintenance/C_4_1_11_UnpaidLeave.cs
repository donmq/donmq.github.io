using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_11_UnpaidLeave : APIController
    {
        private readonly I_4_1_11_UnpaidLeave _service;
        public C_4_1_11_UnpaidLeave(I_4_1_11_UnpaidLeave service)
        {
            _service = service;
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _service.GetListDivision(language));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string language)
        {
            return Ok(await _service.GetListFactory(division, language));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string language)
        {
            return Ok(await _service.GetListDepartment(division, factory, language));
        }

        [HttpGet("GetListLeaveReason")]
        public async Task<IActionResult> GetListLeaveReason(string language)
        {
            return Ok(await _service.GetListLeaveReason(language));
        }

        [HttpGet("GetEmployeeID")]
        public async Task<IActionResult> GetEmployeeID()
        {
            return Ok(await _service.GetEmployeeID());
        }

        [HttpGet("GetEmployeeData")]
        public async Task<IActionResult> GetEmployeeData(string factory, string employeeID, string language)
        {
            return Ok(await _service.GetEmployeeData(factory, employeeID, language));
        }

        [HttpGet("GetSeq")]
        public async Task<IActionResult> GetSeq(string division, string factory, string employeeID)
        {
            var result = await _service.GetSeq(division, factory, employeeID);
            return Ok(result);
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] UnpaidLeaveParam param)
        {
            var result = await _service.GetDataPagination(pagination, param, roleList);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] UnpaidLeaveParam param)
        {
            var result = await _service.DownloadExcel(param, roleList, userName);
            return Ok(result);
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] AddAndEditParam param)
        {
            var result = await _service.AddNew(param, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] AddAndEditParam param)
        {
            var result = await _service.Edit(param, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Emp_Unpaid_LeaveDto data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }
    }
}