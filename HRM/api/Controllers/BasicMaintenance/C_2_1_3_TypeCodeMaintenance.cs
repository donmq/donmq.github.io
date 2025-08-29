using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{

    public class C_2_1_3_TypeCodeMaintenance : APIController
    {
        private readonly I_2_1_3_TypeCodeMaintenance _services;

        public C_2_1_3_TypeCodeMaintenance(I_2_1_3_TypeCodeMaintenance services)
        {
            _services = services;
        }
        [HttpGet("getData")]
        public async Task<IActionResult> GetDataMainPagination([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Basic_Code_TypeParam param)
        {
            var result = await _services.Getdata(pagination, param);
            return Ok(result);
        }
 
        [HttpPost("addNew")]
        public async Task<IActionResult> AddNew([FromBody] Language_Dto param) {
            var result = await _services.AddNew(param,userName);
            return Ok(result);
        }

        [HttpPost("addTypeCode")]
        public async Task<IActionResult> AddTypeCode([FromBody] HRMS_Basic_Code_TypeDto param) {

            var result = await _services.AddTypeCode(param,userName);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Basic_Code_TypeDto param) {
            param.Update_By = userName;
            var result = await _services.Edit(param);
            return Ok(result);
        }
        [HttpPut("editLanguageCode")]
        public async Task<IActionResult> EditLanguageCode([FromBody] Language_Dto param) {
            var result = await _services.EditLanguageCode(param,userName);
            return Ok(result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string type_Seq)
        {
            var result = await _services.Delete(type_Seq);
            return Ok(result);
        }
        [HttpGet("GetLanguage")]
        public async Task<IActionResult> GetLanguage(){
            return Ok(await _services.GetLanguage());
        }
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string type_Seq)
        {
            return Ok(await _services.GetDetail(type_Seq));
        }
    }
}