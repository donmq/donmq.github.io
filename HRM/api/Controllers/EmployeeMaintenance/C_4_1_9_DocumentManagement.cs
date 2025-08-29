using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
  public class C_4_1_9_DocumentManagement : APIController
  {
    private readonly I_4_1_9_DocumentManagement _service;
    public C_4_1_9_DocumentManagement(I_4_1_9_DocumentManagement service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] DocumentManagement_MainParam param)
    {
      var result = await _service.GetDropDownList(param);
      return Ok(result);
    }
    [HttpGet("GetEmployeeList")]
    public async Task<IActionResult> GetEmployeeList([FromQuery] DocumentManagement_SubParam param)
    {
      var result = await _service.GetEmployeeList(param);
      return Ok(result);
    }
    [HttpGet("GetSearchDetail")]
    public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] DocumentManagement_MainParam filter)
    {
      var result = await _service.GetSearchDetail(param, filter, roleList);
      return Ok(result);
    }
    [HttpGet("GetSubDetail")]
    public async Task<IActionResult> GetSubDetail([FromQuery] DocumentManagement_SubParam param)
    {
      var result = await _service.GetSubDetail(param);
      return Ok(result);
    }
    [HttpPost("DownloadFile")]
    public async Task<IActionResult> DownloadFile([FromBody] DocumentManagement_DownloadFileModel param)
    {
      var result = await _service.DownloadFile(param);
      return Ok(result);
    }
    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData([FromQuery] DocumentManagement_SubModel param)
    {
      var result = await _service.CheckExistedData(param);
      return Ok(result);
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] DocumentManagement_SubMemory data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] DocumentManagement_SubMemory data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData([FromQuery] DocumentManagement_SubModel data)
    {
      var result = await _service.DeleteData(data, userName);
      return Ok(result);
    }
    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] DocumentManagement_MainParam param)
    {
      var result = await _service.DownloadExcel(param, roleList);
      return Ok(result);
    }
  }
}