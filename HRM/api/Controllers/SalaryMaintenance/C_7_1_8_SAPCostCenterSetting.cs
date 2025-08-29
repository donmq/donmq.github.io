using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance;

public class C_7_1_8_SAPCostCenterSettingController : APIController
{
    private readonly I_7_1_8_SAPCostCenterSetting _service;

    public C_7_1_8_SAPCostCenterSettingController(I_7_1_8_SAPCostCenterSetting service)
    {
        _service = service;
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory([FromQuery] string language)
           => Ok(await _service.GetListFactory(userName, language));
   
    [HttpGet("GetListKind")]
    public async Task<IActionResult> GetListKind([FromQuery] string language)
           => Ok(await _service.GetListKind(language));

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] PaginationParam pagination, [FromQuery] D_7_8_SAPCostCenterSettingParam param)
    {
        return Ok(await _service.Search(pagination, param));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] D_7_8_SAPCostCenterSettingDto request)
    {
        var result = await _service.AddAsync(request);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] D_7_8_SAPCostCenterSettingDto request)
    {
        var result = await _service.UpdateAsync(request);
        return Ok(result);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(D_7_8_DeleteParam param)
    {
        var result = await _service.DeleteAsync(param);
        return Ok(result);
    }

    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData([FromQuery] D_7_8_CheckDuplicateParam param)
    {
        return Ok(await _service.CheckExistedDataOrCostCenter(param));
    }
   
    [HttpGet("CheckExistedCostCenter")]
    public async Task<IActionResult> CheckExistedCostCenter(D_7_8_CheckDuplicateParam param)
    {
        return Ok(await _service.CheckExistedDataOrCostCenter(param));
    }
  
    [HttpGet("CheckExistedDataCompanyCode")]
    public async Task<IActionResult> CheckExistedDataCompanyCode(string factory, string companyCode)
    {
        return Ok(await _service.CheckExistedDataCompanyCode(factory,companyCode));
    }

    [HttpGet("DownloadExcelTemplate")]
    public async Task<IActionResult> DownloadExcelTemplate()
    {
        return Ok(await _service.DownloadExcelTemplate());
    }

    [HttpGet(template: "ExcelExport")]
    public async Task<ActionResult> ExcelExport([FromQuery] D_7_8_SAPCostCenterSettingParam param)
    {
        var result = await _service.ExcelExport(param, roleList, userName);
        return Ok(result);
    }

    [HttpPost("UploadExcel")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        return Ok(await _service.UploadExcel(file, roleList, userName));
    }

}
