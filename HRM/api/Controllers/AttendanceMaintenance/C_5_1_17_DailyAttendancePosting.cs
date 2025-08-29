using API._Services.Interfaces.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_17_DailyAttendancePosting: APIController
    {
        private readonly I_5_1_17_DailyAttendancePosting _service;
        public C_5_1_17_DailyAttendancePosting(I_5_1_17_DailyAttendancePosting service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, roleList);
            return Ok(result);
        }

        [HttpGet("Execute")]
        public async Task<IActionResult> Execute(string factory)
        {
            var result = await _service.Execute(factory, userName);
            return Ok(result);
        }
    }
}