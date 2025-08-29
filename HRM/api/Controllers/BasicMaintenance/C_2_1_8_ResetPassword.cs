using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_8_ResetPassword : APIController
    {
        private readonly I_2_1_8_ResetPassword _service;

        public C_2_1_8_ResetPassword(I_2_1_8_ResetPassword service)
        {
            _service = service;
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordParam param)
        {
            var result = await _service.ResetPassword(param);
            return Ok(result);
        }
    }
}