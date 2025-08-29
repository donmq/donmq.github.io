
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
  public class C_6_1_4_NewEmployeesCompulsoryInsurancePremium : APIController
  {
    private readonly I_6_1_4_NewEmployeesCompulsoryInsurancePremium _service;
    public C_6_1_4_NewEmployeesCompulsoryInsurancePremium(I_6_1_4_NewEmployeesCompulsoryInsurancePremium service)
    {
      _service = service;
    }
    [HttpGet("GetDropDownList")]
    public async Task<IActionResult> GetDropDownList([FromQuery] NewEmployeesCompulsoryInsurancePremium_Param param)
    {
      var result = await _service.GetDropDownList(param, roleList);
      return Ok(result);
    }
    [HttpGet("Process")]
    public async Task<IActionResult> Process([FromQuery] NewEmployeesCompulsoryInsurancePremium_Param param)
    {
      var result = await _service.Process(param, userName);
      return Ok(result);
    }
  }
}