

using API._Services.Interfaces;
using API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.HomeMain
{
    public class HomeMainController : APIController
    {
        private readonly IHomeMain _service;

        public HomeMainController(IHomeMain service)
        {
            _service = service;
        }

        [HttpGet("GetListPlayers")]
        public async Task<IActionResult> GetListPlayers()
        {
            return Ok(await _service.GetListPlayers());
        }
        [HttpGet("GetListExercise")]
        public async Task<IActionResult> GetListExercise()
        {
            return Ok(await _service.GetListExercise());
        }
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] HomeMainParam param)
        {
            return Ok(await _service.GetData(param));
        }
        [HttpGet("GetListThuocTinh")]
        public async Task<IActionResult> GetListThuocTinh(int ExerciseID, string Position)
        {
            return Ok(await _service.GetListThuocTinh(ExerciseID, Position));
        }
        [HttpGet("GetExercisesForAttributes")]
        public async Task<IActionResult> GetExercisesForAttributes(string key)
        {
            return Ok(await _service.GetExercisesForAttributes(key));
        }
        [HttpGet("GetListDisable")]
        public async Task<IActionResult> GetListDisable(string Position)
        {
            return Ok(await _service.GetListDisable(Position));
        }
        [HttpGet("GetKeys")]
        public async Task<IActionResult> GetKeys()
        {
            return Ok(await _service.GetKeys());
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DataCreate data)
        {
            return Ok(await _service.Create(data));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] DataCreate data)
        {
            return Ok(await _service.Update(data));
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
        [HttpGet("GetListCompares")]
        public async Task<IActionResult> GetListCompares(int inforID)
        {
            return Ok(await _service.GetListCompares(inforID));
        }
        [HttpPost("CreateCompare")]
        public async Task<IActionResult> CreateCompare([FromBody] DataCreate data)
        {
            return Ok(await _service.CreateCompare(data));
        }
        [HttpDelete("DeleteCompare")]
        public async Task<IActionResult> DeleteCompare([FromBody] Quality data)
        {
            return Ok(await _service.DeleteCompare(data));
        }
    }
}