using API._Services.Interfaces.RewardandPenaltyMaintenance;
using API.DTOs.RewardandPenaltyMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RewardandPenaltyMaintenance;


public class C_8_1_1_RewardAndPenaltyReasonCodeMaintenanceController : APIController
{
    private readonly I_8_1_1_RewardAndPenaltyReasonCodeMaintenance _service;
    public C_8_1_1_RewardAndPenaltyReasonCodeMaintenanceController(I_8_1_1_RewardAndPenaltyReasonCodeMaintenance service)
    {
        _service = service;
    }
    [HttpGet("GetData")]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] RewardandPenaltyMaintenanceParam param)
    {
        var result = await _service.GetDataPagination(pagination, param);
        return Ok(result);
    }

    [HttpGet("GetListFactory")]
    public async Task<IActionResult> GetListFactory(string language)
    {
        return Ok(await _service.GetListFactory(userName, language));
    }
    [HttpGet("GetListReason")]
    public async Task<IActionResult> GetListReason(string factory)
    {
        return Ok(await _service.Query_Reason(factory));
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] RewardandPenaltyMaintenance_form data)
    {
        var result = await _service.Create(data);
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] RewardandPenaltyMaintenanceDTO data)
    {
        var result = await _service.Update(data);
        return Ok(result);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] RewardandPenaltyMaintenanceDTO data)
    {
        var result = await _service.Delete(data);
        return Ok(result);
    }
    [HttpGet("DownloadFileExcel")]
    public async Task<IActionResult> Download([FromQuery] RewardandPenaltyMaintenanceParam param)
    {
        var result = await _service.DownloadFileExcel(param, userName);
        return Ok(result);
    }
    
    [HttpGet("IsDuplicatedData")]
    public async Task<IActionResult> IsDuplicatedData([FromQuery] RewardandPenaltyMaintenanceDTO param)
    {
      return Ok(await _service.IsDuplicatedData(param));
    }
}
