using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ViewConfirmDailyController : ControllerBase
    {
        private readonly IViewConfirmDailyService _viewConfirmDailyService;

        public ViewConfirmDailyController(IViewConfirmDailyService viewConfirmDailyService)
        {
            _viewConfirmDailyService = viewConfirmDailyService;
        }

        [HttpGet("GetViewConfirmDaily")]
        public async Task<IActionResult> GetViewConfirmDaily(string lang, string dateFrom, string dateTo, [FromQuery] PaginationParam pagination)
        {
            var result = await _viewConfirmDailyService.GetViewConfirmDaily(lang, dateFrom, dateTo, pagination);
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] ViewConfirmDailyParam param, [FromQuery] PaginationParam pagination)
        {
            var result = await _viewConfirmDailyService.ExportToData(param, pagination);
            return Ok(result);
        }

    }
}