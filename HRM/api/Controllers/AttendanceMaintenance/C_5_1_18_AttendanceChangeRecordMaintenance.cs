using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance;

public class C_5_1_18_AttendanceChangeRecordMaintenance : APIController
{
    private readonly I_5_1_18_AttendanceChangeRecordMaintenance _service;
    public C_5_1_18_AttendanceChangeRecordMaintenance(I_5_1_18_AttendanceChangeRecordMaintenance service)
    {
        _service = service;
    }
    [HttpGet("GetData")]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Att_Change_Record_Params param)
    {
        return Ok(await _service.GetDataPagination(pagination, param));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] HRMS_Att_Change_RecordDto request, string lang)
    {
        var result = await _service.AddAsync(request, userName, lang);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] HRMS_Att_Change_RecordDto request, string lang)
    {
        var result = await _service.UpdateAsync(request, lang);
        return Ok(result);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] HRMS_Att_Change_RecordDto param)
    {
        var result = await _service.DeleteAsync(param);
        return Ok(result);
    }

    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData(string Factory, string Att_Date, string Employee_ID)
    {
        return Ok(await _service.CheckExistedData(Factory, Att_Date, Employee_ID));
    }

    [HttpGet("GetListFactoryByUser")]
    public async Task<IActionResult> GetListFactoryByUser(string language)
    {
        return Ok(await _service.GetListFactoryByUser(language, userName));
    }
}
