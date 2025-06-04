using API._Services.Interfaces.Manage;
using API.Dtos.Manage.PositionManage;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{
    public class PositionManageController : ApiController
    {
        private readonly IPositionManageService _positionManageService;

        public PositionManageController(IPositionManageService positionManageService)
        {
            _positionManageService = positionManageService;
        }

        [HttpGet("GetAllPosition")]
        public async Task<IActionResult> GetAllPosition([FromQuery] PaginationParam pagination)
        {
            var data = await _positionManageService.GetAllPosition(pagination, true);
            return Ok(data);
        }

        [HttpGet("ExportExcelAll")]
        public async Task<IActionResult> ExportExcelAll([FromQuery] PaginationParam pagination, [FromQuery] PositionManageDto dto)
        {
            var result = await _positionManageService.Download(dto);
            return Ok(result);
        }

        [HttpPost("AddPosition")]
        public async Task<IActionResult> AddPosition([FromBody] PositionManageDto PositionAndPosLang)
        {
            var data = await _positionManageService.AddPosition(PositionAndPosLang);
            return Ok(data);
        }

        [HttpDelete("RemovePosition")]
        public async Task<IActionResult> RemovePosition([FromQuery] int IDPosition)
        {
            var data = await _positionManageService.RemovePosition(IDPosition);
            return Ok(data);
        }

        [HttpPut("EditPosition")]
        public async Task<IActionResult> EditPosition([FromBody] PositionManageDto PositionAndPosLang)
        {
            var data = await _positionManageService.EditPosition(PositionAndPosLang);
            return Ok(data);
        }

        [HttpGet("GetDetailPosition")]
        public async Task<IActionResult> GetDetailPosition([FromQuery] int IDPosition)
        {
            var data = await _positionManageService.GetDetailPosition(IDPosition);
            return Ok(data);
        }
    }
}