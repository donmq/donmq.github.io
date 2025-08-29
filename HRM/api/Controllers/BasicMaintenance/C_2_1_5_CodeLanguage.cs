using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_5_CodeLanguage : APIController
    {
        private readonly I_2_1_5_CodeLanguage _service;

        public C_2_1_5_CodeLanguage(I_2_1_5_CodeLanguage service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam paginationParams,[FromQuery] Code_LanguageParam param, string language_Code)
        {
            var data = await _service.Search(paginationParams, param, language_Code);
            return Ok(data);
        }

        [HttpGet("GetDetail")]
        public async Task<ActionResult> GetDetail([FromQuery] Code_LanguageParam param)
        {
            var data = await _service.GetDetail(param);
            return Ok(data);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] Code_LanguageDetail model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Edit(model);
            return Ok(result);
        }
        
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] Code_LanguageDetail model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Detele([FromQuery] Code_LanguageParam param)
        {          
            var data = await _service.Delete(param);
            return Ok(data);
        }

        [HttpGet("Export")]
        public async Task<ActionResult> ExportExcel([FromQuery] Code_LanguageParam param, string language_Code)
        {
            var result = await _service.ExportExcel( param, language_Code );
            return Ok(result);
        }

        [HttpGet("GetTypeSeq")]
        public async Task<IActionResult> GetTypeSeq()
        {
            var result = await _service.GetTypeSeq();
            return Ok(result);
        }

        [HttpGet("GetLanguage")]
        public async Task<IActionResult> GetLanguage(){
            return Ok(await _service.GetLanguage());
        }

        [HttpGet("GetCode")]
        public async Task<IActionResult> GetCode(string type_Seq){
            return Ok(await _service.GetCode(type_Seq));
        }

        [HttpGet("GetCodeName")]
        public async Task<IActionResult> GetCodeName(string type_Seq, string code, string language_Code){
            return Ok(await _service.GetCodeName(type_Seq, code, language_Code));
        }
    }
}