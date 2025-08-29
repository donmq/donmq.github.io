using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_4_DependentInformation : APIController
    {
        private readonly I_4_1_4_DependentInformation _services;
        public C_4_1_4_DependentInformation(I_4_1_4_DependentInformation services)
        {
            _services = services;
        }
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] HRMS_Emp_DependentParam param)
        {
            var result = await _services.GetData(param);
            return Ok(result);
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] HRMS_Emp_DependentDto model)
        {   
            var result = await _services.AddNew(model, userName);
            return Ok(result);
        }
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] HRMS_Emp_DependentDto model)
        {
            var result = await _services.Edit(model, userName);
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Emp_DependentDto model)
        {
            var result = await _services.Delete(model);
            return Ok(result);
        }

        [HttpGet("GetListRelationship")]
        public async Task<IActionResult> GetListRelationship([FromQuery] string language)
        {
            var result = await _services.GetListRelationship(language);
            return Ok(result);
        }
        [HttpGet("GetSeqMax")]
        public async Task<IActionResult> GetSeqMax([FromQuery] HRMS_Emp_DependentDto model)
        {
            var result = await _services.GetMaxSeq(model);
            return Ok(result);
        }
    }
}