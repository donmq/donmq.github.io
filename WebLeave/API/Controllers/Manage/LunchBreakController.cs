using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    public class LunchBreakController : ApiController
    {
        private readonly ILunchBreakService _service;

        public LunchBreakController(ILunchBreakService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LunchBreakDto dto)
        {
            dto.CreatedBy = int.Parse(UserId);
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LunchBreakDto dto)
        {
            dto.UpdatedBy = int.Parse(UserId);
            return Ok(await _service.Update(dto));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            return Ok(await _service.Delete(Id));
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination)
        {
            return Ok(await _service.GetDataPagination(pagination, true));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] int Id)
        {
            return Ok(await _service.GetDetail(Id));
        }

        [HttpGet("GetListLunchBreak")]
        public async Task<IActionResult> GetListLunchBreak()
        {
            return Ok(await _service.GetListLunchBreak());
        }
    }
}