using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_1_RoleSetting : APIController
    {
        private readonly I_2_1_1_RoleSetting _service;
        public C_2_1_1_RoleSetting(I_2_1_1_RoleSetting service)
        {
            _service = service;
        }
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList(string lang)
        {
            var result = await _service.GetDropDownList(lang);
            return Ok(result);
        }
        [HttpGet("GetSearchDetail")]
        public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] RoleSettingParam filter)
        {
            var result = await _service.GetSearchDetail(param, filter);
            return Ok(result);
        }
        [HttpGet("GetProgramGroupDetail")]
        public async Task<IActionResult> GetProgramGroupDetail([FromQuery] RoleSettingParam param)
        {
            var data = await _service.GetProgramGroupDetail(param);
            return Ok(data);
        }
        [HttpGet("GetRoleSettingEdit")]
        public async Task<IActionResult> GetRoleSettingEdit([FromQuery] RoleSettingParam param)
        {
            var data = await _service.GetRoleSettingEdit(param);
            return Ok(data);
        }
        [HttpGet("GetProgramGroupTemplate")]
        public async Task<IActionResult> GetProgramGroupTemp(string lang)
        {
            var data = await _service.GetProgramGroupTemplate(lang);
            return Ok(data);
        }
        [HttpPost("PostRole")]
        public async Task<IActionResult> PostRole([FromBody] RoleSettingDto data)
        {
            var result = await _service.PostRole(data, userName);
            return Ok(result);
        }
        [HttpPost("PutRole")]
        public async Task<IActionResult> PutRole([FromBody] RoleSettingDto data)
        {
            var result = await _service.PutRole(data, userName);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] RoleSettingParam param)
        {
            var result = await _service.DownloadExcel(param);
            return Ok(result);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] string role, string factory)
        {
            var data = await _service.DeleteRole(role, factory);
            return Ok(data);
        }
        [HttpGet("CheckRole")]
        public async Task<IActionResult> CheckRole(string role)
        {
            return Ok(await _service.CheckRole(role, roleList));
        }
    }
}