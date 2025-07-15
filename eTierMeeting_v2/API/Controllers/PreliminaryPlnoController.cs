using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class PreliminaryPlnoController : ApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPreliminaryPlnoService _preliminaryPlno;

        public PreliminaryPlnoController(IPreliminaryPlnoService preliminaryPlno, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _preliminaryPlno = preliminaryPlno;
        }


        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(string search)
        {
            var data = await _preliminaryPlno.ExportExcel(search);

            var dataSheetTwo = await _preliminaryPlno.ExportPreminaryInLocationBuildingExcel();

            var dataSheeThree = await _preliminaryPlno.ExportPreminaryOtherLocationBuildingExcel();

            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot\\Resources\\Template\\PreliminaryPlnoExport.xlsx");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(path);

            Worksheet ws = designer.Workbook.Worksheets[0];
            _preliminaryPlno.PutStaticValue(ref ws);
            designer.SetDataSource("result", data);
            designer.Process();
            // for (int i = 10; i <= data.Count + 10; i++)
            // {
            //     Cell cellCustomSoKiem = ws.Cells["J" + i];
            //     _preliminaryPlno.CustomStyle(ref cellCustomSoKiem);
            // }
            ws.AutoFitColumns();
            ws.PageSetup.CenterHorizontally = true;
            ws.PageSetup.FitToPagesWide = 1;
            ws.PageSetup.FitToPagesTall = 0;


            Worksheet wsheet1 = designer.Workbook.Worksheets[1];
            _preliminaryPlno.PutStaticAllInBuildingValue(designer, ref wsheet1, dataSheetTwo);
            //designer.SetDataSource("result", dataSheetTwo);
            designer.Process();

            Worksheet wsheet2 = designer.Workbook.Worksheets[2];
            _preliminaryPlno.PutStaticAllOtherBuildingValue(designer, ref wsheet2, dataSheeThree);
            //designer.SetDataSource("result", dataSheetTwo);
            designer.Process();

            MemoryStream stream = new MemoryStream();
            designer.Workbook.Save(stream, SaveFormat.Xlsx);

            // designer.Workbook.Save (path + "Test.xlsx", SaveFormat.Xlsx);

            byte[] result = stream.ToArray();

            return File(result, "application/xlsx", "PreliminaryPlnoExport" + DateTime.Now.ToString("yyyy_MM_dd") + ".xlsx");
        }

        [HttpGet("GetAllPreliminaryPlno")]
        public async Task<IActionResult> GetAllPreliminaryPlno([FromQuery] PaginationParams paginationParams, string search)
        {
            var result = await _preliminaryPlno.GetAllPreliminaryPlno(paginationParams, search);
            return Ok(result);
        }


        [HttpPost("AddPreliminaryPlno/{lang}")]
        public async Task<IActionResult> AddPreliminaryPlno([FromBody] PreliminaryPlnoAddDTO preliminaryPlno, string lang)
        {

            var result = await _preliminaryPlno.AddPreliminaryPlno(preliminaryPlno, lang);
            return Ok(result);
        }
        [HttpPost("UpdatePreliminaryPlno/{lang}")]
        public async Task<IActionResult> UpdatePreliminaryPlno([FromBody] PreliminaryPlnoAddDTO preliminaryPlno, string lang)
        {
            var result = await _preliminaryPlno.UpdatePreliminaryPlno(preliminaryPlno, lang);
            return Ok(result);
        }
        [HttpPost("RemovePreliminaryPlno/{empNumber}/{lang}")]
        public async Task<IActionResult> RemovePreliminaryPlno(string empNumber, string lang)
        {
            var result = await _preliminaryPlno.RemovePreliminaryPlno(empNumber, lang);
            return Ok(result);
        }
        [HttpGet("GetPreliminaryPlnos")]
        public async Task<IActionResult> GetPreliminaryPlnos(string empNumber)
        {
            var result = await _preliminaryPlno.GetPreliminaryPlnos(empNumber);
            return Ok(result);
        }
    }
}