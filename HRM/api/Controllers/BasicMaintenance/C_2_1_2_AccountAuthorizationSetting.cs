using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_2_AccountAuthorizationSetting : APIController
    {
        private readonly I_2_1_2_AccountAuthorizationSetting _service;

        public C_2_1_2_AccountAuthorizationSetting(I_2_1_2_AccountAuthorizationSetting service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] AccountAuthorizationSetting_Param param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Download([FromQuery] AccountAuthorizationSetting_Param param)
        {
            var result = await _service.DownloadFileExcel(param);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AccountAuthorizationSetting_Data data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AccountAuthorizationSetting_Data data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Update(data);
            return Ok(result);
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] AccountAuthorizationSetting_Data data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.ResetPassword(data);
            return Ok(result);
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string language)
        {
            return Ok(await _service.GetListDepartment(division, factory, language));
        }


        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _service.GetListDivision(language));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language));
        }

        [HttpGet("GetListRole")]
        public async Task<IActionResult> GetListRole()
        {
            return Ok(await _service.GetListRole());
        }
    }
}