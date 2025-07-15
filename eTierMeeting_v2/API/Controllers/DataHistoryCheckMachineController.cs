using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class DataHistoryCheckMachineController : ApiController
    {
        private readonly IDataHistoryCheckMachineService _dataHistoryCheckMachineService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DataHistoryCheckMachineController(IDataHistoryCheckMachineService dataHistoryCheckMachineService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _dataHistoryCheckMachineService = dataHistoryCheckMachineService;

        }

        [HttpPost("SearchMachine")]
        public async Task<IActionResult> SearchMachine([FromQuery] PaginationParams paginationParams, [FromBody] SearchMachineParams searchMachineParams)
        {
            var result = await _dataHistoryCheckMachineService.SearchMachine(paginationParams, searchMachineParams);
            return Ok(result);
        }

        [HttpPost("MachineExportExcel")]
        public async Task<IActionResult> MachineExportExcel([FromQuery] PaginationParams paginationParams, SearchMachineParams searchMachineParams)
        {
            var data = await _dataHistoryCheckMachineService.ExportExcelMachine(paginationParams, searchMachineParams);

            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ListMachineTemplate.xlsx");
            WorkbookDesigner designer = new()
            {
                Workbook = new Workbook(path)
            };

            Worksheet ws = designer.Workbook.Worksheets[0];

            designer.SetDataSource("result", data);
            designer.Process();

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "DataMachine" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx");
        }

        [HttpGet("SetLanguage")]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return Ok(new OperationResult { Success = true, Message = "Success" });
        }
    }
}