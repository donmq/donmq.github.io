using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_2_6_DailyDinnerAllowanceList : APIController
  {
    private readonly I_5_2_6_DailyDinnerAllowanceList _service;
    public C_5_2_6_DailyDinnerAllowanceList(I_5_2_6_DailyDinnerAllowanceList service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] DailyDinnerAllowanceList_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] DailyDinnerAllowanceList_Param param)
    {
      var result = await _service.Search(param);
      return Ok(result);
    }
    [HttpGet("Excel")]
    public async Task<IActionResult> Excel([FromQuery] DailyDinnerAllowanceList_Param param)
    {
      var result = await _service.Excel(param, userName);
      return Ok(result);
    }
  }
}