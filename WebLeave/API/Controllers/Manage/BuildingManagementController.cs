using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    public class BuildingManagementController : ApiController
    {
        private readonly IBuildingManagementService _service;
        public BuildingManagementController(IBuildingManagementService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        [HttpGet("GetListArea")]
        public async Task<IActionResult> GetListArea()
        {
            var data = await _service.GetListArea();
            return Ok(data);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BuildingInformation model)
        {
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] BuildingInformation model)
        {
            var result = await _service.Edit(model);
            return Ok(result);
        }
    }
}