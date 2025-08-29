using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_2_7_DailyUnregisteredOvertime : APIController
  {
    private readonly I_5_2_7_DailyUnregisteredOvertimeList _service;
    public C_5_2_7_DailyUnregisteredOvertime(I_5_2_7_DailyUnregisteredOvertimeList service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] DailyUnregisteredOvertimeList_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] DailyUnregisteredOvertimeList_Param param)
    {
      var result = await _service.Search(param);
      return Ok(result);
    }
    [HttpGet("Excel")]
    public async Task<IActionResult> Excel([FromQuery] DailyUnregisteredOvertimeList_Param param)
    {
      var result = await _service.Excel(param, userName);
      return Ok(result);
    }
  }
}