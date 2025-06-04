using System.Security;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{
    public class PermissionRightsController : ApiController
    {
        private readonly IPermissionRightsService _service;
        public PermissionRightsController(IPermissionRightsService service){
            _service = service;
        }
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] PermissionParam param)
        {
            var result = await _service.GetDataPagination( pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListParts")]
        public async Task<IActionResult> GetListParts()
        {
            var result = await _service.GetListParts();
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PermissionParam param)
        {
            var result = await _service.ExportExcel(param);
            return Ok(result);
        }

    }
}