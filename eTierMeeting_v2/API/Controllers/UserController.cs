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
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly LocalizationService _languageService;

        public UserController(IUserService userService, IWebHostEnvironment webHostEnvironment, LocalizationService languageService)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _languageService = languageService;
        }

        [HttpGet("GetAllUserPreliminary")]
        public async Task<IActionResult> GetAllUserPreliminary()
        {
            var result = await _userService.GetAllUserPreliminary();
            return Ok(result);
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.GetAllUser();
            return Ok(result);
        }

        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName(string userName)
        {
            var result = await _userService.GetUserName(userName);
            return Ok(result);
        }


        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserDto userDto, string lang)
        {
            var userName = User.FindFirst("UserName").Value;
            var result = await _userService.AddUser(userDto, userName, lang);
            return Ok(result);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto userDto, string lang)
        {
            var userName = User.FindFirst("UserName").Value;
            var result = await _userService.UpdateUser(userDto, userName, lang);
            return Ok(result);
        }
        [HttpPut("RestoreUser")]
        public async Task<IActionResult> RestoreUser(string empNumber, string lang)
        {
            var userNameUpdate = User.FindFirst("UserName").Value;
            var result = await _userService.RestoreUser(empNumber, userNameUpdate, lang);
            return Ok(result);
        }

        [HttpPost("RemoveUser")]
        public async Task<IActionResult> RemoveUser(string empNumber, string lang)
        {
            var sessionUsername = User.FindFirst("UserName").Value;
            var result = await _userService.RemoveUser(empNumber, sessionUsername, lang);
            return Ok(result);
        }

        [HttpPost("SearchUser")]
        public async Task<IActionResult> SearchUser([FromQuery] PaginationParams paginationParams, string keyword)
        {
            var result = await _userService.SearchUser(paginationParams, keyword);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _userService.GetAllUser();
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\ExportUser.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];

            _userService.PutStaticValue(ref ws);
            //Set the data source.
            designer.SetDataSource("result", data);
            designer.Process();
            ws.Cells["G1"].PutValue(_languageService.GetLocalizedHtmlString("inventory_type"));
            ws.Cells["G2"].PutValue(_languageService.GetLocalizedHtmlString("sokiem") + " : ");
            ws.Cells["G3"].PutValue(_languageService.GetLocalizedHtmlString("phuc_kiem") + " : ");
            ws.Cells["G4"].PutValue(_languageService.GetLocalizedHtmlString("rut_kiem") + " : ");
            ws.Cells["H2"].PutValue("3");
            ws.Cells["H3"].PutValue("4");
            ws.Cells["H4"].PutValue("5");
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();

            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();
            return File(result, "application/xlsx", "ExportUser-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
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