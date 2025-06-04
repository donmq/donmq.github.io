using System.Security.Claims;
using API._Services.Interfaces.Leave;
using API.Dtos.Leave;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leave
{
    public class LeaveDetailController : ApiController
    {
        private readonly ILeaveDetailService _leaveDetailService;

        public LeaveDetailController(ILeaveDetailService leaveDetailService)
        {
            _leaveDetailService = leaveDetailService;
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> GetDetail([FromQuery] int leaveID)
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _leaveDetailService.GetDetail(leaveID, userID);
            return Ok(result);
        }

        [HttpGet("RequestEditLeave")]
        public async Task<IActionResult> RequestEditLeave([FromQuery] int? leaveID, [FromQuery] string ReasonAdjust)
        {
            var result = await _leaveDetailService.RequestEditLeave(leaveID, ReasonAdjust);
            return Ok(result);
        }

        [HttpPut("EditApproval")]
        public async Task<IActionResult> EditApproval([FromBody] LeaveEditApprovalDto leaveEditApproval)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data = await _leaveDetailService.EditApproval(userName, userID, leaveEditApproval.empId, leaveEditApproval.slEditApproval, leaveEditApproval.leaveID, leaveEditApproval.notiText);
            return Ok(data);
        }

        [HttpPut("EditCommentArchive")]
        public async Task<IActionResult> EditCommentArchive([FromBody] LeaveEditCommentArchiveDto leaveEditCommentArchive)
        {
            var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data = await _leaveDetailService.EditCommentArchive(userID, leaveEditCommentArchive.leaveID, leaveEditCommentArchive.commentArchiveID);
            return Ok(data);
        }

        [HttpPut("SendNotitoUser")]
        public async Task<IActionResult> SendNotitoUser([FromBody] LeaveSendNotiUser leaveSerndNoti)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var data = await _leaveDetailService.SendNotitoUser(leaveSerndNoti.empId, userName, leaveSerndNoti.notitext, leaveSerndNoti.leaveID);
            return Ok(data);
        }
    }
}