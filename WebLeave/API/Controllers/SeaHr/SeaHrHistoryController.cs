using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr.SeaHrHistory;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{
    public class SeaHrHistoryController : ApiController
    {
        private readonly ISeaHrHistoryService _seaHrHistoryService;

        public SeaHrHistoryController(ISeaHrHistoryService seaHrHistoryService)
        {
            _seaHrHistoryService = seaHrHistoryService;
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] string lang)
        {
            var data = await _seaHrHistoryService.GetCategory(lang);
            return Ok(data);
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var data = await _seaHrHistoryService.GetDepartments();
            return Ok(data);
        }

        [HttpGet("GetPart")]
        public async Task<IActionResult> GetPart([FromQuery] int ID)
        {
            var data = await _seaHrHistoryService.GetPart(ID);
            return Ok(data);
        }

        [HttpGet("GetLeaveData")]
        public async Task<IActionResult> GetLeaveData([FromQuery] SearchHistoryParamsDto paramsSearch, [FromQuery] PaginationParam pagination)
        {
            var data = await _seaHrHistoryService.GetLeaveData(paramsSearch, pagination);
            return Ok(data);
        }

        [HttpGet("ExportExcelAll")]
        public async Task<IActionResult> ExportExcelAll([FromQuery] SearchHistoryParamsDto paramsSearch, [FromQuery] PaginationParam pagination)
        {
            var result = await _seaHrHistoryService.ExportExcel(pagination, paramsSearch);
            return Ok(result);
        }
    }
}