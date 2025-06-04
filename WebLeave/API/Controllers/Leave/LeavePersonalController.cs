
using System.Security.Claims;
using API._Services.Interfaces.Leave;
using API.Dtos.Leave.Personal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leave
{
    public class LeavePersonalController : ApiController
    {
        private readonly ILeavePersonalService _leavePersonalService;

        public LeavePersonalController(ILeavePersonalService leavePersonalService)
        {
            _leavePersonalService = leavePersonalService;
        }

        [HttpPost("AddLeaveDataPersonal")]
        public async Task<IActionResult> AddLeaveDataPersonal(LeavePersonalDto leavePersonalDto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _leavePersonalService.AddLeaveDataPersonal(leavePersonalDto, userId));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId != null && userId != "123")
                return Ok(await _leavePersonalService.GetData(userId));
            else
                return Ok(null);
        }
        [AllowAnonymous]
        [HttpGet("GetDataDetail")]
        public async Task<IActionResult> GetDataDetail(string empNumber)
        {
            var data = await _leavePersonalService.GetDataDetail(empNumber);
            if (data != null)
                return Ok(data);
            else
                return Ok(null);
        }

        [HttpDelete("DeleteLeaveDataPerson")]
        public async Task<IActionResult> DeleteLeaveDataPerson(int leaveId, int empId)
        {
            return Ok(await _leavePersonalService.DeleteLeaveDataPerson(leaveId, empId));
        }
    }
}