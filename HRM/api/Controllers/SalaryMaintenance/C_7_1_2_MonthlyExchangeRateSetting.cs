using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
  public class C_7_1_2_MonthlyExchangeRateSettingController : APIController
  {
    private readonly I_7_1_2_MonthlyExchangeRateSetting _service;
    public C_7_1_2_MonthlyExchangeRateSettingController(I_7_1_2_MonthlyExchangeRateSetting service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] MonthlyExchangeRateSetting_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("IsDuplicatedData")]
    public async Task<IActionResult> IsDuplicatedData([FromQuery] MonthlyExchangeRateSetting_Main param)
    {
      return Ok(await _service.IsDuplicatedData(param));
    }
    [HttpGet("IsExistedData")]
    public async Task<IActionResult> IsExistedData([FromQuery] MonthlyExchangeRateSetting_Main param)
    {
      return Ok(await _service.IsExistedData(param));
    }
    [HttpGet("GetSearchDetail")]
    public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] MonthlyExchangeRateSetting_Param filter)
    {
      var result = await _service.GetSearchDetail(param, filter);
      return Ok(result);
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] MonthlyExchangeRateSetting_Update data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] MonthlyExchangeRateSetting_Update data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData(MonthlyExchangeRateSetting_Main data)
    {
      var result = await _service.DeleteData(data);
      return Ok(result);
    }
  }
}