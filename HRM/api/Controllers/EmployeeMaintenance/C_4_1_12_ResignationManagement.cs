using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_12_ResignationManagement : APIController
    {
        private readonly I_4_1_12_ResignationManagement _service;

        public C_4_1_12_ResignationManagement(I_4_1_12_ResignationManagement service)
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

        [HttpGet("GetListResignationType")]
        public async Task<IActionResult> GetListResignationType(string language)
        {
            return Ok(await _service.GetListResignationType(language));
        }

        [HttpGet("GetListResignReason")]
        public async Task<IActionResult> GetListResignReason(string language, string resignationType)
        {
            return Ok(await _service.GetListResignReason(language, resignationType));
        }

        [HttpGet("GetEmployeeID")]
        public async Task<IActionResult> GetEmployeeID()
        {
            return Ok(await _service.GetEmployeeID());
        }

        [HttpGet("GetEmployeeData")]
        public async Task<IActionResult> GetEmployeeData(string factory, string employeeID)
        {
            return Ok(await _service.GetEmployeeData(factory, employeeID));
        }

        [HttpGet("GetVerifierName")]
        public async Task<IActionResult> GetVerifierName(string factory, string verifier)
        {
            return Ok(await _service.GetVerifierName(factory, verifier));
        }

        [HttpGet("GetVerifierTitle")]
        public async Task<IActionResult> GetVerifierTitle(string factory, string verifier, string language)
        {
            return Ok(await _service.GetVerifierTitle(factory, verifier, language));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] ResignationManagementParam param)
        {
            var result = await _service.GetDataPagination(pagination, param, roleList);
            return Ok(result);
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] ResignAddAndEditParam param)
        {
            var result = await _service.AddNew(param, userName);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] ResignAddAndEditParam param)
        {
            var result = await _service.Edit(param, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Emp_ResignationDto data)
        {
            var result = await _service.Delete(data, userName);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] ResignationManagementParam param)
        {
            var result = await _service.DownloadExcel(param, roleList);
            return Ok(result);
        }
    }
}