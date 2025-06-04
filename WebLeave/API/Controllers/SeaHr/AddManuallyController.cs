using System.Security.Claims;
using API._Services.Services.SeaHr;
using API.Helpers.Params.Seahr;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{
    public class AddManuallyController : ApiController
    {
        private readonly IAddManuallyService _service;
        public AddManuallyController(IAddManuallyService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddManually([FromBody] AddManuallyParam param)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var data = await _service.AddManually(param, userId);
            return Ok(data);
        }

        [HttpDelete("DeleteManually")]
        public async Task<IActionResult> DeleteManually(string leaveId)
        {
            var result = await _service.DeleteManual(leaveId);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(int leaveId)
        {
            var result = await _service.GetDetail(leaveId);
            return Ok(result);
        }

        [HttpGet("Category")] 
        public async Task<IActionResult> GetAllCategory(string languageId) => Ok(await _service.GetAllCategory(languageId));

        [HttpGet("GetCountRestAgent")]
        public async Task<IActionResult> GetCountRestAgent(int year, string empNumber)
        {
            return Ok(await _service.GetCountRestAgent(year, empNumber));
        }

        [HttpGet("CheckDateLeave")]
        public async Task<IActionResult> CheckDateLeave(string start, string end, string empNumber)
        {
            var result = await _service.CheckDateLeave(start, end, empNumber);
            return Ok(new { result });
        }
        [HttpGet("CheckIsSun")]
        public async Task<IActionResult> CheckIsSun(string empNumber)
        {
            var result = await _service.CheckIsSun(empNumber);
            return Ok(result);
        }
    }
}