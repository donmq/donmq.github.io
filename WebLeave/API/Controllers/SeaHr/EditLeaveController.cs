using System.Security.Claims;
using API._Services.Interfaces.SeaHr;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    [Route("Api/[Controller]")]
    [ApiController]

    public class EditLeaveController : ControllerBase
    {
        private readonly IEditLeaveService _editLeaveService;

        public EditLeaveController(IEditLeaveService editLeaveService)
        {
            _editLeaveService = editLeaveService;
        }

        [HttpGet("GetAllEditLeave")]
        public async Task<IActionResult> GetAllEditLeave([FromQuery] PaginationParam param)
        {
            var data = await _editLeaveService.GetAllEditLeave(param);
            return Ok(data);
        }

        [HttpPut("AcceptEditLeave")]
        public async Task<IActionResult> AcceptEditLeave([FromBody] int LeaveID)
        {
            var UserName = User.FindFirst(ClaimTypes.Name).Value;
            var data = await _editLeaveService.AcceptEditLeave(LeaveID, UserName);
            return Ok(data);
        }

        [HttpGet("GetDetailEmployee")]
        public async Task<IActionResult> GetDetailEmployee([FromQuery]int EmployeeID)
        {
            var data = await _editLeaveService.GetDetailEmployee(EmployeeID);
            return Ok(data);
        }

        [HttpPut("RejectEditLeave")]
        public async Task<IActionResult> RejectEditLeave([FromBody] int LeaveID)
        {
            var UserName = User.FindFirst(ClaimTypes.Name).Value;
            var data = await _editLeaveService.RejectEditLeave(LeaveID, UserName);
            return Ok(data);
        }
    }
}
