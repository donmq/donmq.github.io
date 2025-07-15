using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class HistoryController : ApiController
    {
        private readonly IHistoryService _historyService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HistoryController(IHistoryService historyService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _historyService = historyService;

        }

        [HttpPost("SearchHistory")]
        public async Task<IActionResult> SearchHistory([FromQuery] PaginationParams paginationParams, SearchHistoryParams searchHistoryParams)
        {
            var result = await _historyService.SearchHistory(paginationParams, searchHistoryParams);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParams paginationParams, SearchHistoryParams searchHistoryParams)
        {
            var data = await _historyService.ExcelHistories(paginationParams, searchHistoryParams);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\HistoryMachine.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);

            Worksheet ws = designer.Workbook.Worksheets[0];

            designer.SetDataSource("result", data);
            designer.Process();

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            // designer.Workbook.Save (path + "Test.xlsx", SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "DataMachine" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx");
        }
    }
}