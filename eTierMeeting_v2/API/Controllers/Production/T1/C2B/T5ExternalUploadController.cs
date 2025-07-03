using System.IO;
using System.Threading.Tasks;
using Aspose.Cells;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.Helpers.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class T5ExternalUploadController : ControllerBase
    {
        private readonly IT5ExternalUploadService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public T5ExternalUploadController(IT5ExternalUploadService service, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _service = service;
        }

        [HttpGet("getData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination)
        {
            var result = await _service.GetData(pagination);
            return Ok(result);
        }

        [HttpPost("uploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _service.UploadExcel(file);
            return Ok(result);
        }

        [HttpGet("downloadExcel")]
        public async Task<IActionResult> DownloadExcel()
        {
            var designer = new WorkbookDesigner();
            var stream = new MemoryStream();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\Transaction\\T5ExternalUpload\\T5_External_Upload_Temp.xlsx");
            designer.Workbook = new Workbook(path);
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            var result = stream.ToArray();
            return await Task.FromResult(File(result, "application/xlsx", "T5ExternalTemplateExample.xlsx"));
        }

    }
}