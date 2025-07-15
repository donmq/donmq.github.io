
using System.Security.Claims;
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class CellPlnoController : ApiController
    {
        private readonly ICellPlnoService _cellPlnoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CellPlnoController(ICellPlnoService cellPlnoService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _cellPlnoService = cellPlnoService;
        }

        [HttpGet("GetAllCellPlno")]
        public async Task<IActionResult> GetAllCellPlno()
        {
            var result = await _cellPlnoService.GetAllCellPlno();
            return Ok(result);
        }

        [HttpGet("GetListPlnoByCellID")]
        public async Task<IActionResult> GetListPlnoByCellID(int cellID)
        {
            var result = await _cellPlnoService.GetListPlnoByCellID(cellID);
            return Ok(result);
        }

        [HttpGet("GetListPlnoByCellIDV2")]
        public async Task<IActionResult> GetListPlnoByCellID(string cellCode)
        {
            var result = await _cellPlnoService.GetListPlanoByCellIDV2(cellCode);
            return Ok(result);
        }

        [HttpGet("GetListPlnoByPDCID")]
        public async Task<IActionResult> GetListPlnoByPDCID(int pdcID)
        {
            var result = await _cellPlnoService.GetListPlnoByPDCID(pdcID);
            return Ok(result);
        }

        [HttpGet("GetListPlnoByBuildingID")]
        public async Task<IActionResult> GetListPlnoByBuildingID(int buildingID)
        {
            var result = await _cellPlnoService.GetListPlnoByBuildingID(buildingID);
            return Ok(result);
        }

        [HttpPost("GetListPlnoByMultipleBuildingID")]
        public async Task<IActionResult> GetListPlnoByMultipleBuildingID([FromBody] string[] listBuildingID)
        {
            var result = await _cellPlnoService.GetListPlnoByMultipleBuildingID(listBuildingID);
            return Ok(result);
        }

        [HttpPost("GetListPlnoByMultipleCellID")]
        public async Task<IActionResult> GetListPlnoByMultipleCellID([FromBody] string[] listCellID)
        {
            var result = await _cellPlnoService.GetListPlnoByMultipleCellID(listCellID);
            return Ok(result);
        }


        [HttpPost("GetListPlnoByMultipleID")]
        public async Task<IActionResult> GetListPlnoByMultipleID([FromBody] FilterListDTO listAll)
        {
            var result = await _cellPlnoService.GetListPlnoByMultipleID(listAll);
            return Ok(result);
        }
        [HttpGet("GetListPlnoByBuildingAndCellID")]
        public async Task<IActionResult> GetListPlnoByBuildingAndCellID(int buildingID, string cellCode)
        {
            var result = await _cellPlnoService.GetListPlnoByBuildingAndCellID(buildingID, cellCode);
            return Ok(result);
        }

        [HttpGet("GetListPlnoByBuildingInventory")]
        public async Task<IActionResult> GetListPlnoByBuildingInventory(string id, string checkGetData)
        {
            var result = await _cellPlnoService.GetListPlnoByBuildingToInventory(id, checkGetData);
            return Ok(result);
        }

        [HttpGet("GetListCellPlno")]
        public async Task<IActionResult> GetlistCellPlno([FromQuery] PaginationParams pagination)
        {
            var result = await _cellPlnoService.GetListCellPlno(pagination);
            return Ok(result);
        }

        [HttpPost("SearchCellPlno")]
        public async Task<IActionResult> SearchCellPlno([FromQuery] PaginationParams pagination, string keyword)
        {
            var result = await _cellPlnoService.searchCellPlno(pagination, keyword);
            return Ok(result);
        }

        [HttpPost("AddCellPlno")]
        public async Task<IActionResult> AddCellPlono(Cell_PlnoDto model, string lang)
        {
            model.UpdateTime = DateTime.Now;
            model.UpdateBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellPlnoService.AddCellPlno(model, lang);
            return Ok(result);
        }
        [HttpPost("RemoveCellPlno")]
        public async Task<IActionResult> RemoveCellPlno(Cell_PlnoDto model, string lang)
        {
            model.UpdateBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellPlnoService.RemoveCellPlno(model, lang);
            return Ok(result);
        }
        [HttpPost("UpdateCellPlno")]
        public async Task<IActionResult> UpdateCellPlno(Cell_PlnoDto model, string lang)
        {
            model.UpdateTime = DateTime.Now;
            model.UpdateBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellPlnoService.UpdateCellPlno(model, lang);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _cellPlnoService.ExportExcelCellPlno();

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\CellPlnoExport.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);

            Worksheet ws = designer.Workbook.Worksheets[0];
            _cellPlnoService.PutStaticValue(ref ws);

            designer.SetDataSource("result", data);
            designer.Process();

            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "CellPlnoExport" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx");
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