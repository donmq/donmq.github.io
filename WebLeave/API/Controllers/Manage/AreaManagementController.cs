using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    public class AreaManagementController : ApiController
    {
        private readonly IAreaManagementService _service;
        public AreaManagementController(IAreaManagementService service) {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            var data = await _service.GetAll();
            return Ok(data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AreaInformation model) {
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] AreaInformation model) {
            var result = await _service.Edit(model);
            return Ok(result);
        }
    }
}