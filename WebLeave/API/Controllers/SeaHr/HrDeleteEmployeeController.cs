using API._Services.Interfaces.SeaHr;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{

    public class HrDeleteEmployeeController : ApiController
    {
        private readonly IHrDeleteEmployeeService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HrDeleteEmployeeController(IHrDeleteEmployeeService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("UploadExcelDelete")]
        public async Task<IActionResult> UploadExcelDelete([FromForm] IFormFile file)
        {
            var result = await _service.UploadExcelDelete(file);
            return Ok(result);
        }

        [HttpGet("DowloadFile")]
        public async Task<IActionResult> DowloadFile()
        {
            var designer = new WorkbookDesigner();
            var stream = new MemoryStream();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\SeaHr\\ListDelete.xlsx");
            designer.Workbook = new Workbook(path);
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            var result = stream.ToArray();
            return await Task.FromResult(File(result, "application/xlsx", "ListDelete.xlsx"));
        }
    }
}