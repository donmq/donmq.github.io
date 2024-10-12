

using API._Services.Interfaces;
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
    }
}