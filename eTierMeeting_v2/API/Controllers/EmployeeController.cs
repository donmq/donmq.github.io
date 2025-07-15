using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("GetEmployeeScanByNumber")]
        public async Task<IActionResult> GetEmployeeScanByNumber(string empNumber)
        {
            var result = await _employeeService.GetEmployeeScanByNumBer(empNumber);
            return Ok(result);
        }

        [HttpPost("AddEmploy")]
        public async Task<IActionResult> AddEmploy(EmployeeDto employee, string lang)
        {
            var userName = User.FindFirst("UserName").Value;
            var result = await _employeeService.AddEmploy(employee, userName, lang);
            return Ok(result);
        }

        [HttpPost("UpdateEmploy")]
        public async Task<IActionResult> UpdateEmploy(EmployeeDto employee, string lang)
        {
            var userName = User.FindFirst("UserName").Value;
            var result = await _employeeService.UpdateEmploy(employee, userName, lang);
            return Ok(result);
        }

        [HttpPost("RemoveEmploy")]
        public async Task<IActionResult> RemoveEmploy(string empNumber, string lang)
        {
            var result = await _employeeService.RemoveEmploy(empNumber, lang);
            return Ok(result);
        }

        [HttpPost("SearchEmploy")]
        public async Task<IActionResult> SearchEmploy([FromQuery] PaginationParams paginationParams, string keyword)
        {
            var result = await _employeeService.SearchEmploy(paginationParams, keyword);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _employeeService.ExportExcelEmploy();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\EmployExport.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];

            _employeeService.PutStaticValue(ref ws);
            //Set the data source.
            designer.SetDataSource("result", data);
            designer.Process();

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();

            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();
            return File(result, "application/xlsx", "ExportEmployee-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
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