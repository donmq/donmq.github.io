using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_18_RehireEvaluationForFormerEmployees : APIController
    {
        private readonly I_4_1_18_RehireEvaluationForFormerEmployees _service;
        public C_4_1_18_RehireEvaluationForFormerEmployees(I_4_1_18_RehireEvaluationForFormerEmployees service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] RehireEvaluationForFormerEmployeesParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RehireEvaluationForFormerEmployeesEvaluation dto)
        {
            dto.Update_By = userName;
            dto.Update_Time = DateTime.Now;
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] RehireEvaluationForFormerEmployeesEvaluation dto)
        {
            dto.Update_By = userName;
            dto.Update_Time = DateTime.Now;
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string nationality, string identification_Number)
        {
            var result = await _service.GetDetail(nationality, identification_Number);
            return Ok(result);
        }

        [HttpGet("GetListResignationType")]
        public async Task<IActionResult> GetListResignationType(string language)
        {
            return Ok(await _service.GetListResignationType(language));
        }
        [HttpGet("GetListReasonforResignation")]
        public async Task<IActionResult> GetListReasonforResignation(string language)
        {
            return Ok(await _service.GetListReasonforResignation(language));
        }
        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _service.GetListDivision(language));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language, string division)
        {
            return Ok(await _service.GetListFactory(language,division));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language,string factory, string division)
        {
            return Ok(await _service.GetListDepartment(language,factory, division));
        }

        [HttpGet("GetListTypeHeadIdentificationNumber")]
        public async Task<IActionResult> GetListTypeHeadIdentificationNumber(string nationality)
        {
            return Ok(await _service.GetListTypeHeadIdentificationNumber(nationality));
        }

    }
}