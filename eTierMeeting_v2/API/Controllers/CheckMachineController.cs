using System.Drawing;
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Machine_API.Helpers.Utilities;
using Machine_API.Resources;

namespace Machine_API.Controllers
{
    public class CheckMachineController : ApiController
    {
        private readonly ICheckMachineService _checkMachineService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly LocalizationService _languageService;

        public CheckMachineController(ICheckMachineService checkMachineService, IWebHostEnvironment webHostEnvironment, LocalizationService languageService)
        {
            _languageService = languageService;
            _checkMachineService = checkMachineService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("GetMachine")]
        public async Task<IActionResult> GetMachine(string idMachine, string lang)
        {
            var result = await _checkMachineService.GetMachine(idMachine, lang);
            return Ok(result);
        }

        [HttpPost("SubmitCheckMachine")]
        public async Task<IActionResult> SubmitCheckMachine(List<CheckMachineDto> listModel)
        {
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;
            var result = await _checkMachineService.SubmitCheckMachine(listModel, userName, empName);
            return Ok(result);
        }

        [HttpPost("SubmitCheckMachineAll")]
        public async Task<IActionResult> SubmitCheckMachineAll(List<CheckMachineDto> listModel)
        {
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;
            var result = await _checkMachineService.SubmitCheckMachineAll(listModel, userName, empName);
            return Ok(result);
        }

        [HttpPost("ExportPDF")]
        public ActionResult ExportFile(ResultHistoryCheckMachineDto data)
        {
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\CheckMachineReportTemplate.xlsx");
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
            _checkMachineService.PutStaticValue(ref ws, data, userName, empName);
            //Set the data source.
            designer.SetDataSource("result", data.listCheckMachine);
            designer.Process();

            for (int i = 8; i <= data.listCheckMachine.Count + 8; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["F" + i];
                _checkMachineService.CustomStyle(ref cellCustomSoKiem);
            }
            PdfSaveOptions saveOptions = new PdfSaveOptions();
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;
            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Pdf);

            byte[] result = stream.ToArray();
            return File(result, "application/pdf", "CheckMachineReportPdf-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
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