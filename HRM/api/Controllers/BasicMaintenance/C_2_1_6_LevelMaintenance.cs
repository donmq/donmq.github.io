using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_6_LevelMaintenance : APIController
    {
        private readonly I_2_1_6_LevelMaintenance _service;
        public C_2_1_6_LevelMaintenance(I_2_1_6_LevelMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] LevelMaintenanceParam param)
        {
            var result = await _service.GetData(pagination, param);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] HRMS_Basic_LevelDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] HRMS_Basic_LevelDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Edit(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] HRMS_Basic_LevelDto model)
        {
            var result = await _service.Delete(model);
            return Ok(result);
        }

        [HttpGet("GetListLevelCode")]
        public async Task<IActionResult> GetListLevelCode(string type, string language)
        {
            var result = await _service.GetListLevelCode(type, language);
            return Ok(result);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] LevelMaintenanceParam param, string lang)
        {
            var result = await _service.ExportExcel(param, lang);
            return Ok(result);
        }


        [HttpGet("GetTypes")]
        public async Task<IActionResult> GetTypes(string language)
        {
            var result = await _service.GetTypes(language);
            return Ok(result);
        }

    }
}