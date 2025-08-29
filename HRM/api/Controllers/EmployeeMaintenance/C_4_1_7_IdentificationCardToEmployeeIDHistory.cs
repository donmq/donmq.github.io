using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_7_IdentificationCardToEmployeeIDHistory : APIController
    {
        private readonly I_4_1_7_IdentificationCardToEmployeeIDHistory _service;
        public C_4_1_7_IdentificationCardToEmployeeIDHistory(I_4_1_7_IdentificationCardToEmployeeIDHistory service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] IdentificationCardToEmployeeIDHistoryParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
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

        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality(string language)
        {
            return Ok(await _service.GetListNationality(language));
        }
    }
}