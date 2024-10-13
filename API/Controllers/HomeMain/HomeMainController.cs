

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
        public async Task<IActionResult> GetListThuocTinh(int IDBaiTap, string ViTri)
        {
            return Ok(await _service.GetListThuocTinh(IDBaiTap, ViTri));
        }
        [HttpGet("GetListDisable")]
        public async Task<IActionResult> GetListDisable(string ViTri)
        {
            return Ok(await _service.GetListDisable(ViTri));
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
    }
}