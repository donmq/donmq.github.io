using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance;

public class C_5_1_8_CardSwipingDataFormatSetting : APIController
{
    private readonly I_5_1_8_CardSwipingDataFormatSetting _service;

    public C_5_1_8_CardSwipingDataFormatSetting(I_5_1_8_CardSwipingDataFormatSetting service)
    {
        _service = service;
    }

    [HttpGet("GetFactoryMain")]
    public async Task<IActionResult> GetFactoryMain([FromQuery] string language)
    {
        return Ok(await _service.GetFactoryMain(language));
    }

    [HttpGet("GetByFactoryAddList")]
    public async Task<IActionResult> GetByFactoryAddList([FromQuery] string language)
    {
        return Ok(await _service.GetFactoryByAccountAndLanguage(userName, language));
    }

    [HttpGet("GetData")]
    public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string factory)
    {
        return Ok(await _service.GetDataPagination(pagination, factory));
    }

    [HttpGet("GetDataByFactory")]
    public async Task<IActionResult> GetDataByFactory([FromQuery] string factory)
    {
        return Ok(await _service.GetDataByFactory(factory));
    }

    [HttpPost("AddNew")]
    public async Task<IActionResult> AddNew([FromBody] HRMS_Att_Swipecard_SetDto param)
    {
        return Ok(await _service.AddNew(param));
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> Edit([FromBody] HRMS_Att_Swipecard_SetDto param)
    {
        return Ok(await _service.Edit(param));
    }
}
