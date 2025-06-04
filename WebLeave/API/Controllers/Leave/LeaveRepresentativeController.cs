using System.Security.Claims;
using API._Services.Interfaces.Leave;
using API.Dtos.Leave.Personal;
using API.Dtos.Leave.Representative;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leave
{
    public class LeaveRepresentativeController : ApiController
    {
        private readonly ILeaveRepresentativeService _leaveRepresentativeService;

        public LeaveRepresentativeController(ILeaveRepresentativeService leaveRepresentativeService)
        {
            _leaveRepresentativeService = leaveRepresentativeService;
        }

        [HttpPost("AddLeaveData")]
        public async Task<IActionResult> AddLeaveData(LeavePersonalDto leavePersonal)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _leaveRepresentativeService.AddLeaveData(leavePersonal, userId));
        }

        [HttpGet("GetDataLeave")]
        public async Task<IActionResult> GetDataLeave()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _leaveRepresentativeService.GetDataLeave(userId));
        }

        [HttpGet("GetEmployeeInfo")]
        public async Task<IActionResult> GetEmployeeInfo(string empNumber)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _leaveRepresentativeService.GetEmployeeInfo(userId, empNumber));
        }

        [HttpGet("GetListOnTime")]
        public async Task<IActionResult> GetListOnTime(int leaveId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _leaveRepresentativeService.GetListOnTime(userId, leaveId));
        }

        [HttpDelete("DeleteLeave")]
        public async Task<IActionResult> DeleteLeave(List<RepresentativeDataViewModel> leaveDatas)
        {
            return Ok(await _leaveRepresentativeService.DeleteLeave(leaveDatas));
        }
    }
}