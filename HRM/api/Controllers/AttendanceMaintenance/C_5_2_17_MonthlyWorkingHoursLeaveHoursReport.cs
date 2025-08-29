using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_2_17_MonthlyWorkingHoursLeaveHoursReport : APIController
  {
    private readonly I_5_2_17_MonthlyWorkingHoursLeaveHoursReport _service;
    public C_5_2_17_MonthlyWorkingHoursLeaveHoursReport(I_5_2_17_MonthlyWorkingHoursLeaveHoursReport service)
    {
      _service = service;
    }

    [HttpGet("GetTotalRows")]
    public async Task<IActionResult> GetTotalRows([FromQuery] MonthlyWorkingHoursLeaveHoursReportParam param)
    {
      param.UserName = userName;
      var result = await _service.GetTotalRows(param);
      return Ok(result);
    }

    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] MonthlyWorkingHoursLeaveHoursReportParam param)
    {
      param.UserName = userName;
      var result = await _service.ExportExcel(param);
      return Ok(result);
    }

    [HttpGet("GetListPermissionGroup")]
    public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
    {
      return Ok(await _service.GetListPermissionGroup(factory, language));
    }
    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory(string language)
    {
      return Ok(await _service.Queryt_Factory_AddList(userName, language));
    }
  }
}