using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
  public class C_7_1_27_FinSalaryAttributionCategoryMaintenanceController : APIController
  {
    private readonly I_7_1_27_FinSalaryAttributionCategoryMaintenance _service;
    public C_7_1_27_FinSalaryAttributionCategoryMaintenanceController(I_7_1_27_FinSalaryAttributionCategoryMaintenance service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] FinSalaryAttributionCategoryMaintenance_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("GetDepartmentList")]
    public async Task<IActionResult> GetDepartmentList([FromQuery] FinSalaryAttributionCategoryMaintenance_Param param)
    {
      var result = await _service.GetDepartmentList(param);
      return Ok(result);
    }
    [HttpGet("GetKindCodeList")]
    public async Task<IActionResult> GetKindCodeList([FromQuery] FinSalaryAttributionCategoryMaintenance_Param param)
    {
      var result = await _service.GetKindCodeList(param);
      return Ok(result);
    }
    [HttpGet("GetSearch")]
    public async Task<IActionResult> GetSearch([FromQuery] PaginationParam param, [FromQuery] FinSalaryAttributionCategoryMaintenance_Param filter)
    {
      var result = await _service.GetSearch(param, filter);
      return Ok(result);
    }
    [HttpGet("IsExistedData")]
    public async Task<IActionResult> IsExistedData([FromQuery] FinSalaryAttributionCategoryMaintenance_Param param)
    {
      return Ok(await _service.IsExistedData(param));
    }
    [HttpPut("PutData")]
    public async Task<IActionResult> PutData([FromBody] FinSalaryAttributionCategoryMaintenance_Data data)
    {
      var result = await _service.PutData(data);
      return Ok(result);
    }
    [HttpPost("PostData")]
    public async Task<IActionResult> PostData([FromBody] FinSalaryAttributionCategoryMaintenance_Update data)
    {
      var result = await _service.PostData(data);
      return Ok(result);
    }
    [HttpDelete("DeleteData")]
    public async Task<IActionResult> DeleteData(FinSalaryAttributionCategoryMaintenance_Data data)
    {
      var result = await _service.DeleteData(data);
      return Ok(result);
    }
    [HttpPost("UploadExcel")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
      var result = await _service.UploadExcel(file, roleList, userName);
      return Ok(result);
    }
    [HttpGet("DownloadExcel")]
    public async Task<ActionResult> DownloadExcel([FromQuery] FinSalaryAttributionCategoryMaintenance_Param param)
    {
      var result = await _service.DownloadExcel(param, userName);
      return Ok(result);
    }
    [HttpGet("DownloadExcelTemplate")]
    public async Task<ActionResult> DownloadExcelTemplate()
    {
      var result = await _service.DownloadExcelTemplate();
      return Ok(result);
    }
  }
}