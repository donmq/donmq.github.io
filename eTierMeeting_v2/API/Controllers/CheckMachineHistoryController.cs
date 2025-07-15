using System.Drawing;
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class CheckMachineHistoryController : ApiController
    {
        private readonly ICheckMachineHistoryService _checkMachineHistory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly LocalizationService _languageService;

        public CheckMachineHistoryController(ICheckMachineHistoryService checkMachineHistory, IWebHostEnvironment webHostEnvironment, LocalizationService languageService)
        {
            _languageService = languageService;
            _checkMachineHistory = checkMachineHistory;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("SearchHistoryCheckMachine")]
        public async Task<IActionResult> SearchHistoryCheckMachine(CheckMachineHisstoryParams checkMachineHisstoryParams, [FromQuery] PaginationParams paginationParams)
        {
            var result = await _checkMachineHistory.SearchHistoryCheckMachine(checkMachineHisstoryParams, paginationParams);
            return Ok(result);
        }

        [HttpPost("GetDetailHistoryCheckMachine")]
        public async Task<IActionResult> GetDetailHistoryCheckMachine(int historyCheckMachineID)
        {
            var result = await _checkMachineHistory.GetDetailHistoryCheckMachine(historyCheckMachineID);
            return Ok(result);
        }

        [HttpPost("ExportFile")]
        public async Task<ActionResult> ExportFileAsync(HistoryCheckMachineDto data)
        {
            var listData = await _checkMachineHistory.GetDetailHistoryCheckMachine(data.HistoryCheckMachineID);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\CheckMachineHistoryTemplate.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];

            Cell cell = ws.Cells["A1"];
            cell.PutValue(_languageService.GetLocalizedHtmlString("machine_check_results"));
            Style style;
            style = cell.GetStyle();
            style.Font.Color = Color.Black;
            cell.SetStyle(style);
            cell.Worksheet.PageSetup.CenterHorizontally = true;
            _checkMachineHistory.PutStaticValue(ref ws, data);
            //Set the data source.
            designer.SetDataSource("result", listData);
            designer.Process();

            for (int i = 8; i <= listData.Count + 8; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["F" + i];
                _checkMachineHistory.CustomStyle(ref cellCustomSoKiem);
            }

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();

            if (data.TypeFile == "excel")
            {
                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                byte[] result = stream.ToArray();
                return File(result, "application/xlsx", "CheckMachineReport-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
            }
            else
            {
                designer.Workbook.Save(stream, SaveFormat.Pdf);
                byte[] result = stream.ToArray();
                return File(result, "application/pdf", "CheckMachineReport-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
            }
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
