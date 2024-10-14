

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
        public async Task<IActionResult> GetListThuocTinh(int BaiTap)
        {
            return Ok(await _service.GetListThuocTinh(BaiTap));
        }
    }
}