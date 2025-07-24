using System.Globalization;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.EmployeeManage;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeService _service;
        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("SearchEmploy")]
        public async Task<IActionResult> SearchEmploy([FromQuery] PaginationParam paginationParams, string keyword, string lang)
        {
            var result = await _service.Search(paginationParams, keyword, lang);
            return Ok(result);
        }

        [HttpPut("UpdateEmploy")]
        public async Task<IActionResult> UpdateEmploy(EmployeeDto employee)
        {
            if (DateTime.TryParseExact(employee.DateIn, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                string formattedDate = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                employee.DateIn = formattedDate;
            }
            var result = await _service.UpdateEmploy(employee);
            return Ok(result);
        }

        [HttpPut("UpdateInDetail")]
        public async Task<IActionResult> UpdateInDetail(EmployExportDto employee)
        {
            var result = await _service.UpdateInDetail(employee);

            return Ok(result);
        }

        [HttpGet("getListGroupBase")]
        public async Task<IActionResult> ListGroupBase()
        {
            var data = await _service.ListGroupBase();
            return Ok(data);
        }

        [HttpGet("getListPositionID")]
        public async Task<IActionResult> ListPositionID()
        {
            var data = await _service.ListPositionID();
            return Ok(data);
        }

        [HttpGet("getListPartID")]
        public async Task<IActionResult> ListPartID(int DeptID)
        {
            var data = await _service.ListPartID(DeptID);
            return Ok(data);
        }

        [HttpGet("getListDeptID")]
        public async Task<IActionResult> ListDeptID()
        {
            var data = await _service.ListDeptID();
            return Ok(data);
        }

        [HttpGet("getDataDetail")]
        public async Task<IActionResult> GetDataDetail(int EmpID, string lang)
        {
            var data = await _service.GetDataDetail(EmpID, lang);
            return Ok(data);
        }

        [HttpGet("SearchDetail")]
        public async Task<IActionResult> SearchDetail([FromQuery] PaginationParam paginationParams, [FromQuery] EmployeeDetalParam param)
        {
            var result = await _service.SearchDetail(paginationParams, param);
            return Ok(result);
        }

        [HttpGet("ListCataLog")]
        public async Task<IActionResult> ListCatelog(string lang)
        {
            var data = await _service.ListCataLog(lang);
            return Ok(data);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var result = await _service.ExportExcelEmploy();
            return Ok(result);
        }

        [HttpDelete("RemoveEmploy")]
        public async Task<IActionResult> RemoveEmploy(int empID)
        {
            var result = await _service.RemoveEmploy(empID);
            return Ok(result);
        }

        [HttpGet("getDisable")]
        public async Task<IActionResult> GetDisabel(int empID)
        {
            var result = await _service.ChangeVisible(empID);
            return Ok(result);
        }

    }
}