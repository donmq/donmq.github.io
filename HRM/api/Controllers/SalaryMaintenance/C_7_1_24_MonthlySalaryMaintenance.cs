using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_24_MonthlySalaryMaintenanceController : APIController
    {
        private readonly I_7_1_24_MonthlySalaryMaintenance _service;
        public C_7_1_24_MonthlySalaryMaintenanceController(I_7_1_24_MonthlySalaryMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] MonthlySalaryMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language, factory));
        }
        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType(string language)
        {
            return Ok(await _service.GetListSalaryType(language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string language )
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }

        [HttpGet("GetListTypeHeadEmployeeID")]
        public async Task<IActionResult> GetListTypeHeadEmployeeID(string factory)
        {
            return Ok(await _service.GetListTypeHeadEmployeeID(factory));
        }

        [HttpGet("GetDetailPersonal")]
        public async Task<IActionResult> GetDetailPersonal(string factory, string employee_ID)
        {
            return Ok(await _service.GetDetailPersonal(factory, employee_ID));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] MonthlySalaryMaintenanceDto item)
        {
            return Ok(await _service.GetDetail(item));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] MonthlySalaryMaintenance_Update dto)
        {          
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] MonthlySalaryMaintenance_Delete dto)
        {
            var result = await _service.Delete(dto);
            return Ok(result);
        }
    }
}