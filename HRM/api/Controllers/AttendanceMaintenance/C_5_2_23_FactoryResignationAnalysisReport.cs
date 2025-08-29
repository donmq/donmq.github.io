
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
  public class C_5_2_23_FactoryResignationAnalysisReport : APIController
  {
    private readonly I_5_2_23_FactoryResignationAnalysisReport _service;
    public C_5_2_23_FactoryResignationAnalysisReport(I_5_2_23_FactoryResignationAnalysisReport service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] FactoryResignationAnalysisReport_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("Process")]
    public async Task<IActionResult> Process([FromQuery] FactoryResignationAnalysisReport_Param param)
    {
      var result = await _service.Process(param, userName);
      return Ok(result);
    }
  }
}