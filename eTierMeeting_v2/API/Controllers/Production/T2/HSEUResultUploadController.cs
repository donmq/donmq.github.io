using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspose.Cells;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO.HSEUpload;
using eTierV2_API.Helpers.Params;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T2
{
    [Route("api/[controller]")]
    [ApiController]
    public class HSEUResultUploadController : ControllerBase
    {
        private readonly IHSEUResultUploadService _serviceHSEResultUpload;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HSEUResultUploadController(IHSEUResultUploadService serviceHSEResultUpload,IWebHostEnvironment webHostEnvironment) {
            _serviceHSEResultUpload = serviceHSEResultUpload;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getBuildings")]
        public async Task<IActionResult> GetBuildings() {
            var data = await _serviceHSEResultUpload.GetBuildings();
            return Ok(data);
        }

        [HttpGet("getDeptInBuilding")]
        public async Task<IActionResult> GetDeptInBuilding(string building) {
            var data = await _serviceHSEResultUpload.GetDeptInBuilding(building);
            return Ok(data);
        }

        [HttpGet("downLoadTemplateExcel")]
        public async Task<IActionResult> DownLoadTemplateExcel() {
            var evalutionCategory = await _serviceHSEResultUpload.GetEvalutionCategory();
            var dataETMUnits = await _serviceHSEResultUpload.GetETMUnits();
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            dataETMUnits.ForEach(item => {
                item.Year = year;
                item.Month = month;
                item.Center_Level = "Production";
                item.Tier_Level = "T2";
                item.Section = "CTB";
            });

            // Get Template
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\HSE_Result_Upload.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);
            Worksheet ws = designer.Workbook.Worksheets[0];

            // gen data evalution category
            var countEvalution = evalutionCategory.Count;
            ws.Cells.Merge(0,8,1,countEvalution);
            ws.Cells[0,8].PutValue("Evalucation Category");
            // this.SetStyleUtility(ws,0,8,Color.FromArgb(224,168,0),Color.FromArgb(255,255,255));

            ArrayList ws_List_evalucation = new ArrayList();
            evalutionCategory.ForEach(x => {ws_List_evalucation.Add(x);});
            ws.Cells.ImportArrayList(ws_List_evalucation,1,8, false);
            for (int i = 0; i < evalutionCategory.Count; i++)
            {
                var cell = ws.Cells[1,8 + i];
                Style style = cell.GetStyle();
                style.IsTextWrapped = true;
                cell.SetStyle(style);
            }

              // gen data
            designer.SetDataSource("result", dataETMUnits);
            designer.Process();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);
            byte[] result = stream.ToArray();
            return File(result, "application/xlsx", "HSE_Result_Upload" + ".xlsx");
        }
        private void SetStyleUtility(Worksheet ws, int row, int column, Color colorBackground, Color fontColor) {
            var cell = ws.Cells[row,column];
            Style style = cell.GetStyle();
            style.ForegroundColor = colorBackground;
            style.Font.Color = fontColor;
            style.Pattern = BackgroundType.Solid;
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.Font.IsBold = true;
            cell.SetStyle(style);
        }

        [HttpPost("uploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file) {
            var updatedBy = User.FindFirst(ClaimTypes.Name).Value;
            var data = await _serviceHSEResultUpload.UploadExcel(file, updatedBy);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery]PaginationParam param, HSESearchParam paramSearch) {
            var result = await _serviceHSEResultUpload.Search(param, paramSearch);
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Remove(int id) {
            var result = await _serviceHSEResultUpload.Remove(id);
            return Ok(result);
        }

        [HttpPut("updateScoreData")]
        public async Task<IActionResult> UpdateScoreData(HSEDataSearchDto model) {
            var updatedBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _serviceHSEResultUpload.UpdateScoreData(model, updatedBy);
            return Ok(result); 
        }

        [HttpPost("uploadImages")]
        public async Task<IActionResult> UploadImages([FromForm]ImageDataUpload data) {
            var updatedBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _serviceHSEResultUpload.UploadImages(data, updatedBy);
            return Ok(result);
        }

        [HttpGet("getListImageToHseID/{id}")]
        public async Task<IActionResult> GetListImageToHseID(int id) {
            var images = await _serviceHSEResultUpload.GetListImageToHseID(id);
            return Ok(images);
        }

        [HttpPost("editImages")]
        public async Task<IActionResult> EditImages([FromForm]ImageDataUpload data) {
            var updatedBy = User.FindFirst(ClaimTypes.Name).Value;
            var result = await _serviceHSEResultUpload.EditImages(data, updatedBy);
            return Ok(result);
        }
    }
}