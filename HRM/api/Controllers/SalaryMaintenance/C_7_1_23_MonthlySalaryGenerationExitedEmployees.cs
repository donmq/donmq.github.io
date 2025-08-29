using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
  public class C_7_1_23_MonthlySalaryGenerationExitedEmployeesController : APIController
  {
    private readonly I_7_1_23_MonthlySalaryGenerationExitedEmployees _service;
    public C_7_1_23_MonthlySalaryGenerationExitedEmployeesController(I_7_1_23_MonthlySalaryGenerationExitedEmployees service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] MonthlySalaryGenerationExitedEmployees_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("CheckCloseStatus")]
    public async Task<IActionResult> CheckCloseStatus([FromQuery] MonthlySalaryGenerationExitedEmployees_Param param)
    {
      var result = await _service.CheckCloseStatus(param);
      return Ok(result);
    }
    [HttpPost("Execute")]
    public async Task<IActionResult> Execute([FromBody] MonthlySalaryGenerationExitedEmployees_Param param)
    {
      var result = await _service.Execute(param, userName);
      return Ok(result);
    }
  }
}