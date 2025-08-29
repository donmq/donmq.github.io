using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
  public class C_7_1_1_SalaryItemAndAmountSettingsController : APIController
  {
    private readonly I_7_1_1_SalaryItemAndAmountSettings _service;
    public C_7_1_1_SalaryItemAndAmountSettingsController(I_7_1_1_SalaryItemAndAmountSettings service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] SalaryItemAndAmountSettings_MainParam param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("IsExistedData")]
    public async Task<IActionResult> IsExistedData([FromQuery] SalaryItemAndAmountSettings_MainData param)
    {
      return Ok(await _service.IsExistedData(param));
    }
    [HttpGet("IsDuplicatedData")]
    public async Task<IActionResult> IsDuplicatedData([FromQuery] SalaryItemAndAmountSettings_SubParam param)
    {
      return Ok(await _service.IsDuplicatedData(param, userName));
    }
    [HttpGet("GetSearchDetail")]
    public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] SalaryItemAndAmountSettings_MainParam filter)
    {
      var result = await _service.GetSearchDetail(param, filter);
      return Ok(result);
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] SalaryItemAndAmountSettings_Update data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] SalaryItemAndAmountSettings_Update data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData(SalaryItemAndAmountSettings_MainData data)
    {
      var result = await _service.DeleteData(data);
      return Ok(result);
    }
  }
}