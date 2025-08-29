using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_1_1_FactoryCalendar : APIController
  {
    private readonly I_5_1_1_FactoryCalendar _service;
    public C_5_1_1_FactoryCalendar(I_5_1_1_FactoryCalendar service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] FactoryCalendar_MainParam param)
    {
      var result = await _service.GetDropDownList(param, userName);
      return Ok(result);
    }
    [HttpGet("GetSearchDetail")]
    public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] FactoryCalendar_MainParam filter)
    {
      var result = await _service.GetSearchDetail(param, filter);
      return Ok(result);
    }
    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData(string Division, string Factory, string Att_Date)
    {
      return Ok(await _service.CheckExistedData(Division, Factory, Att_Date));
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] FactoryCalendar_Table data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] FactoryCalendar_Table data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData([FromBody] FactoryCalendar_Table data)
    {
      var result = await _service.DeleteData(data);
      return Ok(result);
    }
    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] FactoryCalendar_MainParam param)
    {
      var result = await _service.DownloadExcel(param);
      return Ok(result);
    }
    [HttpGet("DownloadExcelTemplate")]
    public async Task<ActionResult> DownloadExcelTemplate()
    {
      var result = await _service.DownloadExcelTemplate();
      return Ok(result);
    }
    [HttpPost("UploadExcel")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
      var result = await _service.UploadExcel(file, roleList, userName);
      return Ok(result);
    }
  }
}