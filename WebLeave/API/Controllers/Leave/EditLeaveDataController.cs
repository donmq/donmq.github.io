using System.Security.Claims;
using API._Services.Interfaces.Leave;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Leave
{
    public class EditLeaveDataController : ApiController
    {
        private readonly IEditLeaveDataService _editLeaveDataService;

        public EditLeaveDataController(IEditLeaveDataService editLeaveDataService)
        {
            _editLeaveDataService = editLeaveDataService;
        }

        [HttpGet("GetAllEditLeaveData")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParam pagination)
        {
            var userID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var list = await _editLeaveDataService.GetAllEditLeave(pagination, userID);
            return Ok(list);
        }

        [HttpPut("EditLeave")]
        public async Task<IActionResult> EditLeave([FromBody] int LeaveID)
        {
            var UserName = User.FindFirst(ClaimTypes.Name).Value;
            var data = await _editLeaveDataService.EditLeaveData(LeaveID, UserName);
            return Ok(data);
        }

        [HttpGet("GetDetailEmployee")]
        public async Task<IActionResult> GetDetailEmployee([FromQuery] string leaveID)
        {
            var data = await _editLeaveDataService.GetDetailEmployee(leaveID);
            return Ok(data);
        }

        [HttpGet("GetLeaveByID")]
        public async Task<IActionResult> GetLeaveByID([FromQuery] string leaveID)
        {
            var data = await _editLeaveDataService.GetLeaveByID(leaveID);
            return Ok(data);
        }
    }
}