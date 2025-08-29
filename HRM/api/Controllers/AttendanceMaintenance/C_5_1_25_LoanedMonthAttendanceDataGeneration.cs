using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_25_LoanedMonthAttendanceDataGeneration : APIController
    {
        private readonly I_5_1_25_LoanedMonthAttendanceDataGeneration _service;

        public C_5_1_25_LoanedMonthAttendanceDataGeneration(I_5_1_25_LoanedMonthAttendanceDataGeneration service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
        {
            return Ok(await _service.GetListFactory(userName, language));
        }

        [HttpPost("Execute")]
        public async Task<IActionResult> Execute([FromBody] LoanedDataGenerationDto dto)
        {
            dto.UserName = userName;
            return Ok(await _service.Execute(dto));
        }
    }
}