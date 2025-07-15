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
    public class InventoryController : ApiController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly LocalizationService _languageService;

        public InventoryController(IInventoryService inventoryService, IWebHostEnvironment webHostEnvironment, LocalizationService languageService)
        {
            _languageService = languageService;
            _inventoryService = inventoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetMachine")]
        public async Task<IActionResult> GetMachine(string idMachine, string lang)
        {
            var result = await _inventoryService.GetMachine(idMachine, lang);
            return Ok(result);
        }
        [HttpPost("GetAllMachineByPlno")]
        public async Task<IActionResult> GetAllMachineByPlno(string plnoID)
        {
            var result = await _inventoryService.GetAllMachineByPlno(plnoID);
            return Ok(result);
        }
        [HttpPost("SubmitInventory")]
        public async Task<IActionResult> SubmitInventory(InventoryParams inventoryParams)
        {
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;
            var result = await _inventoryService.SubmitInventory(inventoryParams, userName, empName);
            return Ok(result);
        }

        [HttpPost("ExportFile")]
        public ActionResult ExportFileAsync(ResultHistoryInventoryDto data)
        {
            var userName = User.FindFirst("UserName").Value;
            var empName = User.FindFirst("EmpName").Value;

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ExportInventorytemplate.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];

            Cell cell = ws.Cells["A1"];
            cell.PutValue(_languageService.GetLocalizedHtmlString("inventory_result"));
            Style style;
            style = cell.GetStyle();
            style.Font.Color = Color.Black;
            cell.SetStyle(style);
            cell.Worksheet.PageSetup.CenterHorizontally = true;
            _inventoryService.PutStaticValue(ref ws, data, userName, empName);
            //Set the data source.
            designer.SetDataSource("result", data.ListInventory);
            designer.Process();

            for (int i = 8; i <= data.ListInventory.Count + 8; i++)
            {
                Cell cellCustomSoKiem = ws.Cells["F" + i];
                _inventoryService.CustomStyle(ref cellCustomSoKiem);
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
                return File(result, "application/xlsx", "ExportInventory-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
            }
            else
            {
                designer.Workbook.Save(stream, SaveFormat.Pdf);
                byte[] result = stream.ToArray();
                return File(result, "application/pdf", "ExportInventory-" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
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