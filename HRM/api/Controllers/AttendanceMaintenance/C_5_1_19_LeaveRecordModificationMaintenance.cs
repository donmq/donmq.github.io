using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance;

public class C_5_1_19_LeaveRecordModificationMaintenance : APIController
{
    private readonly I_5_1_19_LeaveRecordModificationMaintenance _service;
    public C_5_1_19_LeaveRecordModificationMaintenance(I_5_1_19_LeaveRecordModificationMaintenance service)
    {
        _service = service;
    }
    [HttpGet("GetData")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] Leave_Record_Modification_MaintenanceSearchParamDto param)
    {
        return Ok(await _service.GetDataPagination(pagination, param, roleList));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Leave_Record_Modification_MaintenanceDto request)
    {
        var result = await _service.AddAsync(request);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Leave_Record_Modification_MaintenanceDto request)
    {
        var result = await _service.UpdateAsync(request);
        return Ok(result);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] Leave_Record_Modification_MaintenanceDto param)
    {
        var result = await _service.DeleteAsync(param, userName);
        return Ok(result);
    }
    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData(string Factory, string Employee_ID, string Leave_Code, string Leave_Date)
    {
        return Ok(await _service.CheckExistedData(Factory, Employee_ID, Leave_Code, Leave_Date));
    }
    [HttpGet("GetListLeave")]
    public async Task<IActionResult> GetListLeave([FromQuery] string language)
    {
        return Ok(await _service.GetListLeave(language));
    }
    [HttpGet("GetListFactoryByUser")]
    public async Task<IActionResult> GetListFactoryByUser(string language)
    {
        return Ok(await _service.GetListFactoryByUser(language, userName));
    }
    [HttpGet("GetWorkShiftType")]
    public async Task<IActionResult> GetWorkShiftType([FromQuery] Leave_Record_Modification_MaintenanceSearchParamDto param)
    {
        var result = await _service.GetWorkShiftType(param);
        return Ok(result);
    }

    [HttpGet("DownloadFileExcel")]
    public async Task<IActionResult> Download([FromQuery] Leave_Record_Modification_MaintenanceSearchParamDto param)
    {
        var result = await _service.DownloadFileExcel(param, userName);
        return Ok(result);
    }
}
