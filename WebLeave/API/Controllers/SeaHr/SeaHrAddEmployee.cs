using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SeaHr
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeaHrAddEmployee : ControllerBase
    {
        private readonly ISeaHrAddEmployeeService _seaHrAddEmployeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SeaHrAddEmployee(ISeaHrAddEmployeeService seaHrAddEmployeeService, IWebHostEnvironment webHostEnvironment)
        {
            _seaHrAddEmployeeService = seaHrAddEmployeeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment()
        {
            var data = await _seaHrAddEmployeeService.GetListDepartment();
            return Ok(data);
        }

        [HttpGet("GetListPosition")]
        public async Task<IActionResult> GetListPosition()
        {
            var data = await _seaHrAddEmployeeService.GetListPosition();
            return Ok(data);
        }

        [HttpGet("GetListPart")]
        public async Task<IActionResult> GetListPart(int departmentId)
        {
            var data = await _seaHrAddEmployeeService.GetListPart(departmentId);
            return Ok(data);
        }

        [HttpGet("GetListGroupBase")]
        public async Task<IActionResult> GetListGroupBase()
        {
            var data = await _seaHrAddEmployeeService.GetListGroupBase();
            return Ok(data);
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            var data = await _seaHrAddEmployeeService.AddNewEmployee(employeeDTO);
            return Ok(data);
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            var result = await _seaHrAddEmployeeService.UploadExcel(file);
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources\\Template\\SeaHr\\AddListEmployee.xlsx");
            var workbook = new Workbook(path);
            var design = new WorkbookDesigner(workbook);
            MemoryStream stream = new();
            design.Workbook.Save(stream, SaveFormat.Xlsx);
            var result = stream.ToArray();
            return await Task.FromResult(File(result, "application/xlsx", "AddListEmployee.xlsx"));
        }
    }
}