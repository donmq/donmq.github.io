
using API._Services.Interfaces.SystemMaintenance;
using API.DTOs.SystemMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SystemMaintenance
{
    public class C_1_1_3_SystemLanguageSetting : APIController
    {
        private readonly I_1_1_3_SystemLanguageSetting _service;
        public C_1_1_3_SystemLanguageSetting(I_1_1_3_SystemLanguageSetting service)
        {
            _service = service;
        }
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination)
        {
            var result = await _service.GetData(pagination);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SystemLanguageSetting_Data data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SystemLanguageSetting_Data data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Update(data);
            return Ok(result);
        }

    }
}