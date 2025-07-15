using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class ReportHistoryInventoryController : ApiController
    {
        private readonly IReportHistoryInventoryService _reportHistoryInventoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHistoryInventoryService _historyInventoryService;
        private readonly ICellPlnoService _cellPlnoService;
        private readonly Resources.LocalizationService _languageService;
        public ReportHistoryInventoryController(
            IReportHistoryInventoryService reportHistoryInventoryService,
            ICellPlnoService cellPlnoService,
            IHistoryInventoryService historyInventoryService,
            IWebHostEnvironment webHostEnvironment,
            Resources.LocalizationService languageService)
        {
            _languageService = languageService;
            _cellPlnoService = cellPlnoService;
            _historyInventoryService = historyInventoryService;
            _webHostEnvironment = webHostEnvironment;
            _reportHistoryInventoryService = reportHistoryInventoryService;

        }

        [HttpPost("ExportFile")]
        public async Task<IActionResult> ExportFile(ReportHistoryInventoryParams data)
        {
            var dataMain = await _historyInventoryService.GetListDetailHistoryPlno(data.PlnoId, data.TimeSokiem, data.TimePhucKiem, data.TimeRutKiem, data.Lang);

            var dataPlno = await _cellPlnoService.GetPlace(data.PlnoId);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ReportHistoryInventory.xlsx");
            WorkbookDesigner designer = new()
            {
                Workbook = new Workbook(path)
            };

            Worksheet ws = designer.Workbook.Worksheets[0];
            Cell cell = ws.Cells["A1"];
            cell.PutValue(_languageService.GetLocalizedHtmlString("inventory_report") + " - " + data.PlnoId + " - " + dataPlno.Place.Trim());
            _reportHistoryInventoryService.PutStaticValue(ref ws, dataMain);
            designer.SetDataSource("result", dataMain.ListResult);
            designer.Process();

            for (int i = 7; i <= dataMain.ListResult.Count + 7; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["F" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomSoKiem);
                Cell cellCustomPhucKiem = ws.Cells["G" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomPhucKiem);
                Cell cellCustomRutKiem = ws.Cells["H" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomRutKiem);
            }
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new();
            if (data.TypeFile == "excel")
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

        [HttpGet("ExportAllInventory")]
        public async Task<IActionResult> ExportAllInventory([FromQuery] ReportKiemKeParam filterParam)
        {
            var dataMain = await _historyInventoryService.GetAllDetailHistoryPlno(filterParam);

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\Inventory_machine_Yearly_checking.xlsx");
            WorkbookDesigner designer = new()
            {
                Workbook = new Workbook(path)
            };

            Worksheet ws = designer.Workbook.Worksheets[0];
            Cell cell = ws.Cells["A1"];
            cell.PutValue(_languageService.GetLocalizedHtmlString("inventory_report_all_location"));
            _reportHistoryInventoryService.PutStaticAllInventoryValue(ref ws, dataMain);
            designer.SetDataSource("result", dataMain.ListResult);
            designer.Process();

            for (int i = 4; i <= dataMain.ListResult.Count + 4; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["F" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomSoKiem);
                Cell cellCustomPhucKiem = ws.Cells["G" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomPhucKiem);
                Cell cellCustomRutKiem = ws.Cells["H" + i];
                _reportHistoryInventoryService.CustomStyle(ref cellCustomRutKiem);
            }
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new();

            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();
            return File(result, "application/xlsx", "Inventory_machine_Yearly_checking_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
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