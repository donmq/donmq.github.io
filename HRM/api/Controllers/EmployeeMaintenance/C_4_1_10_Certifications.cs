using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
  public class C_4_1_10_Certifications : APIController
  {
    private readonly I_4_1_10_Certifications _service;
    public C_4_1_10_Certifications(I_4_1_10_Certifications service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] Certifications_MainParam param)
    {
      var result = await _service.GetDropDownList(param);
      return Ok(result);
    }
    [HttpGet("GetEmployeeList")]
    public async Task<IActionResult> GetEmployeeList([FromQuery] Certifications_SubParam param)
    {
      var result = await _service.GetEmployeeList(param);
      return Ok(result);
    }
    [HttpGet("GetSearchDetail")]
    public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] Certifications_MainParam filter)
    {
      var result = await _service.GetSearchDetail(param, filter, roleList);
      return Ok(result);
    }
    [HttpGet("GetSubDetail")]
    public async Task<IActionResult> GetSubDetail([FromQuery] Certifications_SubParam param)
    {
      var result = await _service.GetSubDetail(param);
      return Ok(result);
    }
    [HttpPost("DownloadFile")]
    public async Task<IActionResult> DownloadFile([FromBody] Certifications_DownloadFileModel param)
    {
      var result = await _service.DownloadFile(param);
      return Ok(result);
    }
    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData([FromQuery] Certifications_SubModel param)
    {
      var result = await _service.CheckExistedData(param);
      return Ok(result);
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] Certifications_SubMemory data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] Certifications_SubMemory data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData([FromQuery] Certifications_SubModel data)
    {
      var result = await _service.DeleteData(data,userName);
      return Ok(result);
    }
    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] Certifications_MainParam param)
    {
      var result = await _service.DownloadExcel(param, roleList);
      return Ok(result);
    }
  }
}