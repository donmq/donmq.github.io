using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance;

public class C_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance : APIController
{
    private readonly I_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance _service;

    public C_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance(I_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance service)
    {
        _service = service;
    }

    [HttpGet("GetData")]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param)
    {
        return Ok(await _service.GetDataPagination(pagination, param));
    }

    [HttpGet("GetListDivision")]
    public async Task<IActionResult> GetListDivision(string language)
    {
        return Ok(await _service.GetListDivision(language));
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory(string division, string language)
    {
        return Ok(await _service.GetListFactory(division, language, userName));
    }

    [HttpGet("GetListWorkType")]
    public async Task<IActionResult> GetListWorkType([FromQuery] string Language)
    {
        return Ok(await _service.GetListWorkType(Language));
    }

    [HttpPost("AddNew")]
    public async Task<IActionResult> AddNew([FromBody] HRMS_Att_Work_Type_DaysDto param)
    {
        return Ok(await _service.AddNew(param));
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> Edit([FromBody] HRMS_Att_Work_Type_DaysDto param)
    {
        return Ok(await _service.Edit(param));
    }
    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] SpecialWorkTypeAnnualLeaveDaysMaintenanceParam  param)
    {
      var result = await _service.DownloadExcel(param,userName);
      return Ok(result);
    }
}
