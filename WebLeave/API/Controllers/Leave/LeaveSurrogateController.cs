using System.Security.Claims;
using API._Services.Interfaces.Leave;
using API.Dtos.Leave;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leave
{
    public class LeaveSurrogateController : ApiController
    {
        private readonly ILeaveSurrogateService _service;

        public LeaveSurrogateController(ILeaveSurrogateService service)
        {
            _service = service;
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] int userId)
        {
            return Ok(await _service.GetDetail(userId));
        }

        [HttpGet("GetSurrogates")]
        public async Task<IActionResult> GetSurrogates([FromQuery] int userId)
        {
            return Ok(await _service.GetSurrogates(userId));
        }

        [HttpPost("SaveSurrogate")]
        public async Task<IActionResult> SaveSurrogate([FromBody] SurrogateDto dto)
        {
            return Ok(await _service.SaveSurrogate(dto));
        }

        [HttpPut("RemoveSurrogate")]
        public async Task<IActionResult> RemoveSurrogate([FromBody] SurrogateRemoveDto dto)
        {
            return Ok(await _service.RemoveSurrogate(dto));
        }
    }
}