using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_13_ExitEmployeesBlacklist : APIController
    {
        private readonly I_4_1_13_ExitEmployeesBlacklist _service;

        public C_4_1_13_ExitEmployeesBlacklist(I_4_1_13_ExitEmployeesBlacklist service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Emp_BlacklistParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Emp_BlacklistDto data)
        {
            var result = await _service.Edit(data, userName);
            return Ok(result);
        }

        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality(string language)
        {
            return Ok(await _service.GetListNationality(language));
        }

        [HttpGet("GetListIdentificationNumber")]
        public async Task<IActionResult> GetListIdentificationNumber()
        {
            return Ok(await _service.GetIdentificationNumber());
        }
    }
}