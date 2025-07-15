using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class HistoryInventoryController : ApiController
    {
        private readonly IHistoryInventoryService _historyInventoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HistoryInventoryController(IHistoryInventoryService historyInventoryService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _historyInventoryService = historyInventoryService;

        }

        [HttpPost("SearchHistoryInventory")]
        public async Task<IActionResult> SearchHistoryInventory([FromQuery] PaginationParams paginationParams, [FromBody] HistoryInventoryParams historyInventoryParams)
        {
            var result = await _historyInventoryService.SearchHistoryInventory(historyInventoryParams, paginationParams);
            return Ok(result);
        }

        [HttpPost("GetDetailHistoryInventory")]
        public async Task<IActionResult> GetDetailHistoryInventory(int historyInventoryID)
        {
            var result = await _historyInventoryService.GetDatalHistoryInventoryById(historyInventoryID);
            return Ok(result);
        }

        [HttpPost("ExportFile")]
        public async Task<IActionResult> ExportFile(HistoryInventoryDto historyInventoryDto)
        {
            var data = await _historyInventoryService.GetDatalHistoryInventoryById(historyInventoryDto.HistoryInventoryID);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ExportHistoryInventory.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);

            Worksheet ws = designer.Workbook.Worksheets[0];
            _historyInventoryService.PutStaticValue(ref ws, historyInventoryDto);
            designer.SetDataSource("result", data);
            designer.Process();

            for (int i = 10; i <= data.Count + 10; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["J" + i];
                _historyInventoryService.CustomStyle(ref cellCustomSoKiem);
            }

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();
            if (historyInventoryDto.TypeFile == "excel")
            {
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                byte[] result = stream.ToArray();
                return File(result, "application/xlsx", "ExportHistoryInventory-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
            }
            else
            {
                designer.Workbook.Save(stream, SaveFormat.Pdf);
                byte[] result = stream.ToArray();
                return File(result, "application/pdf", "ExportHistoryInventory-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
            }
        }

        [HttpPost("CheckDate")]
        public IActionResult CheckDateHistoryInventory(string checkDate)
        {
            var isCheck = _historyInventoryService.CheckDateHistoryInventory(checkDate);
            return Ok(isCheck);
        }

        [HttpPost("GetListPlnoHistotry")]
        public async Task<IActionResult> GetListPlnoHistotry(string dateSearch, int idBuilding, int? isCheck)
        {
            var result = await _historyInventoryService.GetListPlnoHistotry(dateSearch, idBuilding, isCheck);
            return Ok(result);
        }

        [HttpPost("GetListDetailHistoryInventory")]
        public async Task<IActionResult> GetListDetailHistoryInventory(string plnoId, string timeSoKiem, string timePhucKiem, string timeRutKiem, string lang)
        {
            var result = await _historyInventoryService.GetListDetailHistoryPlno(plnoId, timeSoKiem, timePhucKiem, timeRutKiem, lang);
            return Ok(result);
        }

        [HttpGet("GetDataPdfByDay")]
        public async Task<IActionResult> GetDataPdfByDay(string date)
        {
            var data = await _historyInventoryService.GetDataPdfByDay(date);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ExportHistoryInventoryPDF.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);

            Worksheet ws = designer.Workbook.Worksheets[0];
            _historyInventoryService.PutStaticValue1(ref ws, data[0]);

            designer.SetDataSource("result", data);
            designer.Process();

            for (int i = 7; i <= data.Count + 7; i++)
            {
                Cell cellCustom = ws.Cells["N" + i];
                _historyInventoryService.CustomStyle1(ref cellCustom);
            }
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Pdf);

            // designer.Workbook.Save (path + "Test.xlsx", SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "ExportHistoryInventoryPDF" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".pdf");
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