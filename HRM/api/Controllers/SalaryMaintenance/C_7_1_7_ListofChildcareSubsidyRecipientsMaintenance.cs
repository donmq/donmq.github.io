using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance;

public class C_7_1_7_ListofChildcareSubsidyRecipientsMaintenanceController : APIController
{
    private readonly I_7_1_7_ListofChildcareSubsidyRecipientsMaintenance _service;

    public C_7_1_7_ListofChildcareSubsidyRecipientsMaintenanceController(I_7_1_7_ListofChildcareSubsidyRecipientsMaintenance service)
    {
        _service = service;
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory([FromQuery] string language)
           => Ok(await _service.GetListFactory(userName, language));

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] PaginationParam pagination, [FromQuery] D_7_7_HRMS_Sal_Childcare_SubsidyParam param)
    {
        return Ok(await _service.Search(pagination, param));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] D_7_7_HRMS_Sal_Childcare_SubsidyDto request)
    {
        var result = await _service.AddAsync(request);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] D_7_7_HRMS_Sal_Childcare_SubsidyDto request)
    {
        var result = await _service.UpdateAsync(request);
        return Ok(result);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(D_7_7_DeleteParam param)
    {
        var result = await _service.DeleteAsync(param);
        return Ok(result);
    }

    [HttpGet("CheckExistedData")]
    public async Task<IActionResult> CheckExistedData(string Factory, string Employee_ID, string Birthday_Child)
    {
        return Ok(await _service.CheckExistedData(Factory, Employee_ID, Birthday_Child));
    }

    [HttpGet("DownloadExcelTemplate")]
    public async Task<IActionResult> DownloadExcelTemplate()
    {
        return Ok(await _service.DownloadExcelTemplate());
    }

    [HttpGet(template: "ExcelExport")]
    public async Task<ActionResult> ExcelExport([FromQuery] D_7_7_HRMS_Sal_Childcare_SubsidyParam param)
    {
        var result = await _service.ExcelExport(param, userName);
        return Ok(result);
    }

    [HttpPost("UploadExcel")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        return Ok(await _service.UploadExcel(file, roleList, userName));
    }

}
