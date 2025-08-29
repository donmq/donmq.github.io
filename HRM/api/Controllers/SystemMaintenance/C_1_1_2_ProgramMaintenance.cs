
using API._Services.Interfaces.SystemMaintenance;
using API.DTOs.SystemMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SystemMaintenance
{
    
    public class C_1_1_2_ProgramMaintenance : APIController
    {
        private readonly I_1_1_2_ProgramMaintenance _services;
        public C_1_1_2_ProgramMaintenance(I_1_1_2_ProgramMaintenance services)
        {
            _services = services;
        }
        [HttpGet("getDirectory")]
        public async Task<IActionResult> GetDirectory()
        {
            var result = await _services.GetDirectory();
            return Ok(result);
        }
        [HttpGet("getData")]
        public async Task<IActionResult> Getdata([FromQuery] PaginationParam pagination,[FromQuery]ProgramMaintenance_Param param)
        {
            var result = await _services.Getdata(pagination,param);
            return Ok(result);
        }
        
        [HttpGet("getFunction_ALL")]
        public async Task<IActionResult> GetFunction_ALL()
        {
            var result = await _services.GetFunction_ALL();
            return Ok(result);
        }
         [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ProgramMaintenance_Data model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _services.AddNew(model);
            
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody]ProgramMaintenance_Data model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _services.Edit(model);
            
            return Ok(result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string Program_Code)
        {
            var result = await _services.Delete(Program_Code);
            return Ok(result);
        }
        
    }
}