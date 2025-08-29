using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_2_ShiftScheduleSetting : APIController
    {
        private readonly I_5_1_2_ShiftScheduleSetting _services;
        public C_5_1_2_ShiftScheduleSetting(I_5_1_2_ShiftScheduleSetting services)
        {
            _services = services;
        }

        [HttpGet("GetDivisions")]
        public async Task<IActionResult> GetDivisions(string language)
        {
            var result = await _services.GetDivisions(language);
            return Ok(result);
        }

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetFactories(language, userName);
            return Ok(result);
        }

        [HttpGet("GetFactoriesByDivision")]
        public async Task<IActionResult> GetFactoriesByDivision(string division, string language)
        {
            var result = await _services.GetFactories(division, language, userName);
            return Ok(result);
        }

        [HttpGet("GetWorkShiftTypes")]
        public async Task<IActionResult> GetWorkShiftTypes(string language)
        {
            var result = await _services.GetWorkShiftTypes(language);
            return Ok(result);
        }


        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam param, [FromQuery] HRMS_Att_Work_ShiftParam filters)
        {
            var result = await _services.GetDataPagination(param, filters);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Att_Work_ShiftDto model)
        {
            var result = await _services.Create(model);
            return Ok(result);
            
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Att_Work_ShiftDto model)
        {
            var result = await _services.Update(model);
            return Ok(result);
        }
    }
}