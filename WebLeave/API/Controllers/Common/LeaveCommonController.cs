using API._Services.Interfaces.Common;
using API.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Common
{
    public class LeaveCommonController : ApiController
    {
        private readonly ILeaveCommonService _leaveCommonService;

        public LeaveCommonController(ILeaveCommonService leaveCommonService)
        {
            _leaveCommonService = leaveCommonService;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory(string language)
        {
            return Ok(await _leaveCommonService.GetAllCategory(language));
        }

        [HttpGet("GetListHoliday")]
        public async Task<IActionResult> GetListHoliday()
        {
            return Ok(await _leaveCommonService.GetListHoliday());
        }

        [HttpGet("GetCountRestAgent")]
        public async Task<IActionResult> GetCountRestAgent(int empId, int year)
        {
            return Ok(await _leaveCommonService.GetCountRestAgent(empId, year));
        }

        [HttpGet("CheckDateLeave")]
        public async Task<IActionResult> CheckDateLeave(string start, string end, int empid)
        {
            var result = await _leaveCommonService.CheckDateLeave(start, end, empid);
            return Ok(new { result });
        }

        [HttpGet("CheckDataDatePicker")]
        public async Task<IActionResult> CheckDataDatePicker()
        {
            return Ok(await _leaveCommonService.CheckDataDatePicker());
        }

        [HttpGet("GetWorkShift")]
        public async Task<IActionResult> GetWorkShift([FromQuery] string shift)
        {
            return Ok(await _leaveCommonService.GetWorkShift(shift));
        }

        [HttpGet("GetWorkShifts")]
        public async Task<IActionResult> GetWorkShifts()
        {
            return Ok(await _leaveCommonService.GetWorkShifts());
        }
    }
}