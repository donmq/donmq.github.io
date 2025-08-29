using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance;

public class C_5_2_10_HRDailyReport : APIController
{
    private readonly I_5_2_10_HRDailyReport _service;
    public C_5_2_10_HRDailyReport(I_5_2_10_HRDailyReport service)
    {
        _service = service;
    }

    [HttpGet("GetTotalRows")]
    public async Task<IActionResult> GetTotalRows([FromQuery] HRDailyReportParam param)
    {
        var result = await _service.GetTotalRows(param, roleList,userName);
        return Ok(result);
    }

    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] HRDailyReportParam param)
    {
        var result = await _service.DownloadExcel(param, roleList, userName);
        return Ok(result);
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory(string lang)
    {
        return Ok(await _service.Queryt_Factory_AddList(userName, lang));
    }

    [HttpGet("GetListLevel")]
    public async Task<IActionResult> GetListLevel(string lang)
    {
        return Ok(await _service.GetListLevel(lang));
    }

    [HttpGet("GetListPermissionGroup")]
    public async Task<IActionResult> GetListPermissionGroup(string factory, string lang)
    {
        return Ok(await _service.GetListPermissionGroup(factory, lang));
    }
}
