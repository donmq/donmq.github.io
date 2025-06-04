using System.Security.Claims;
using API._Services.Interfaces.Leave;
using API.Dtos.Leave.LeaveHistory;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Leave
{
    public class LeaveHistoryController : ApiController
    {
        private readonly ILeaveHistoryService _leaveHistoryService;
        public LeaveHistoryController(ILeaveHistoryService leaveHistoryService)
        {
            _leaveHistoryService = leaveHistoryService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchHistoryParamsDto paramsSearch, [FromQuery] PaginationParam pagination, bool isPaging = true)
        {
            paramsSearch.UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _leaveHistoryService.GetLeaveData(paramsSearch, pagination, isPaging);
            return Ok(result);
        }

        [HttpGet("GetListCategory")]
        public async Task<IActionResult> GetListCategory([FromQuery] string lang)
        {
            var data = await _leaveHistoryService.GetCategory(lang);
            return Ok(data);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] HistoryExportParam param, [FromQuery] PaginationParam pagination)
        {
            var result = await _leaveHistoryService.ExportExcel(param, pagination);
            return Ok(result);
        }

    }
}