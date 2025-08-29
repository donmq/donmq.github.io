using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance;

public class C_7_1_25_MonthlySalaryMaintenanceExitedEmployeesController : APIController
{
    private readonly I_7_1_25_MonthlySalaryMaintenanceExitedEmployees _service;

    public C_7_1_25_MonthlySalaryMaintenanceExitedEmployeesController(I_7_1_25_MonthlySalaryMaintenanceExitedEmployees service)
    {
        _service = service;
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory(string language)
    {
        return Ok(await _service.GetListFactory(userName, language));
    }

    [HttpGet("GetListPermissionGroup")]
    public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
    {
        return Ok(await _service.GetListPermissionGroup(factory, language));
    }

    [HttpGet("GetListSalaryType")]
    public async Task<IActionResult> GetListSalaryType(string language)
    {
        return Ok(await _service.GetListSalaryType(language));
    }

    [HttpGet("GetListDepartment")]
    public async Task<IActionResult> GetListDepartment(string factory, string language)
    {
        var result = await _service.GetListDepartment(factory, language);
        return Ok(result);
    }

    [HttpGet("Get_MonthlyAttendanceData_MonthlySalaryDetail")]
    public async Task<IActionResult> Get_MonthlyAttendanceData_MonthlySalaryDetail([FromQuery] D_7_25_GetMonthlyAttendanceDataDetailParam param)
    {
        var result = await _service.Get_MonthlyAttendanceData_MonthlySalaryDetail(param);
        return Ok(result);
    }

    [HttpGet("GetDataPagination")]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] D_7_25_MonthlySalaryMaintenanceExitedEmployeesSearchParam param)
    {
        var result = await _service.GetDataPagination(pagination, param);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] D_7_25_MonthlySalaryMaintenance_Update dto)
    {
        var result = await _service.Update(dto);
        return Ok(result);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain dto)
    {
        var result = await _service.Delete(dto);
        return Ok(result);
    }   
}
