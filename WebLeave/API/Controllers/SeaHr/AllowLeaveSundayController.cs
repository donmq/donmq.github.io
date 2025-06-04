using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{
    public class AllowLeaveSundayController : ApiController
    {
        private readonly IAllowLeaveSundayService _service;
        public AllowLeaveSundayController(IAllowLeaveSundayService service)
        {
            _service = service;
        }

        [HttpGet("GetPagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] AllowLeaveSundayParam param)
        {
            var result = await _service.GetPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromQuery] AllowLeaveSundayParam param)
        {
            var result = await _service.GetEmployee(param);
            return Ok(result);
        }

        [HttpPut("DisallowLeave")]
        public async Task<IActionResult> DisallowLeave([FromBody] int EmpID)
        {
            var result = await _service.DisallowLeave(EmpID);
            return Ok(result);
        }

        [HttpGet("GetParts")]
        public async Task<IActionResult> GetParts()
        {
            var result = await _service.GetParts();
            return Ok(result);
        }

        [HttpPut("AllowLeave")]
        public async Task<IActionResult> AllowLeave([FromBody] List<int> EmpSelected)
        {
            var result = await _service.AllowLeave(EmpSelected);
            return Ok(result);
        }
    }
}