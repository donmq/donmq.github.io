using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_2_5_DailySwipeCardAnomaliesList : APIController
  {
    private readonly I_5_2_5_DailySwipeCardAnomaliesList _service;
    public C_5_2_5_DailySwipeCardAnomaliesList(I_5_2_5_DailySwipeCardAnomaliesList service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] DailySwipeCardAnomaliesList_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] DailySwipeCardAnomaliesList_Param param)
    {
      var result = await _service.Search(param);
      return Ok(result);
    }
    [HttpGet("Excel")]
    public async Task<IActionResult> Excel([FromQuery] DailySwipeCardAnomaliesList_Param param)
    {
      var result = await _service.Excel(param, userName);
      return Ok(result);
    }
  }
}