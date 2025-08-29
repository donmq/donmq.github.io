using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_5_ExternalExperience : APIController
    {
        private readonly I_4_1_5_ExternalExperience _service;

        public C_4_1_5_ExternalExperience(I_4_1_5_ExternalExperience service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] HRMS_Emp_External_ExperienceParam filter)
        {
            var result = await _service.GetData(filter);
            return Ok(result);
        }

        [HttpGet("GetSeq")]
        public async Task<IActionResult> GetSeq([FromQuery] HRMS_Emp_External_ExperienceDto data)
        {
            var result = await _service.GetMaxSeq(data);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Emp_External_ExperienceDto data)
        {

            var result = await _service.AddNew(data, userName);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Emp_External_ExperienceDto data)
        {

            var result = await _service.Edit(data, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Emp_External_ExperienceDto data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }
    }
}