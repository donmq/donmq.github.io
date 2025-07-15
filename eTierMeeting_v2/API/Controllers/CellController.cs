
using System.Security.Claims;
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class CellController : ApiController
    {
        private readonly ICellService _cellService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CellController(ICellService cellService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _cellService = cellService;

        }

        [HttpGet("GetAllCell")]
        public async Task<IActionResult> GetAllCell()
        {
            var result = await _cellService.GetAllCell();
            return Ok(result);
        }

        [HttpGet("GetAllCellAdmin")]
        public async Task<IActionResult> GetAllCellAdmin()
        {
            var result = await _cellService.GetAllCellAdmin();
            return Ok(result);
        }

        [HttpGet("GetListCellByBuildingID")]
        public async Task<IActionResult> GetListCellByBuildingID(int buildingID)
        {
            var result = await _cellService.GetListCellByBuildingID(buildingID);
            return Ok(result);
        }

        [HttpGet("GetListCellExistPlnoByBuildingID")]
        public async Task<IActionResult> GetListCellExistPlnoByBuildingID(int buildingID)
        {
            var result = await _cellService.GetListCellExistPlnoByBuildingID(buildingID);
            return Ok(result);
        }
        [HttpGet("GetListCellByPdcID")]
        public async Task<IActionResult> GetListCellByPdcID(int pdcId)
        {
            var result = await _cellService.GetListCellByPdcID(pdcId);
            return Ok(result);
        }

        [HttpGet("GetListCell")]
        public async Task<IActionResult> GetListCell([FromQuery] PaginationParams paginationParams)
        {
            var result = await _cellService.GetListCell(paginationParams);
            return Ok(result);
        }

        [HttpPost("SearchCell")]
        public async Task<IActionResult> SearchCell([FromQuery] PaginationParams paginationParams, string keyword)
        {
            var result = await _cellService.SearchCell(paginationParams, keyword);
            return Ok(result);
        }

        [HttpPost("AddCell")]
        public async Task<IActionResult> AddCell(CellDto model, string lang)
        {
            model.UpdateTime = DateTime.Now;
            model.UpdateBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellService.AddCell(model, lang);
            return Ok(result);
        }

        [HttpPost("RemoveCell")]
        public async Task<IActionResult> RemoveCell(CellDto model, string lang)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellService.RemoveCell(model, lang);
            return Ok(result);
        }

        [HttpPost("UpdateCell")]
        public async Task<IActionResult> UpdateCell(CellDto model, string lang)
        {
            model.UpdateTime = DateTime.Now;
            model.UpdateBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _cellService.UpdateCell(model, lang);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _cellService.ExportExcelCell();

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\CellExport.xlsx");
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

            return File(result, "application/xlsx", "CellExport" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx");
        }

        [HttpGet("GetDataCell")]
        public async Task<IActionResult> GetDataCell(string cellCode)
        {
            var result = await _cellService.GetDataCell(cellCode);
            return Ok(result);
        }

    }
}