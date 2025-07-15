
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class AssetsLendMaintainController : ApiController
    {
        private readonly IAssetsLendMaintainService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AssetsLendMaintainController(IAssetsLendMaintainService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParams pagination, [FromQuery] AssetsLendMaintainParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }
        [HttpGet("GetListLendTo")]
        public async Task<IActionResult> GetListLendTo()
        {
            var result = await _service.GetListLendTo();
            return Ok(result);
        }
        [HttpPost("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] AssetsLendMaintainParam param)
        {
            var data = await _service.DownloadExcel(param);
            return File(data, "application/xlsx", "AssetsLendMaintainExport" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx");
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update(AssetsLendMaintainDto data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _service.UploadExcel(file);
            return Ok(result);
        }
    }
}