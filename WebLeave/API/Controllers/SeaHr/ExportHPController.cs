using API._Services.Interfaces.SeaHr;
using API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    public class ExportHPController : ApiController
    {
        private readonly IExportHPService _service;
        public ExportHPController(IExportHPService service)
        {
            _service = service;
        }

        [HttpPost("GetData")]
        public async Task<IActionResult> Search([FromQuery] PaginationParam paginationParam, ExportHPParam param)
        {
            var data = await _service.PaginationData(param, paginationParam);
            return Ok(data);
        }

        [HttpGet("ExportHPExcel")]
        public async Task<IActionResult> ExportHPExcel([FromQuery] ExportHPParam param, [FromQuery] string typeFile)
        {
            var result = await _service.DownloadExcel(param, typeFile);
            return Ok(result);
        }
    }
}