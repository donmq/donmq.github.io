using API._Services.Interfaces.SystemMaintenance;
using API.DTOs;
using API.DTOs.SystemMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SystemMaintenance
{
    public class C_1_1_1_DirectoryMaintenance : APIController
    {
        private readonly I_1_1_1_DirectoryMaintenance _service;

        public C_1_1_1_DirectoryMaintenance(I_1_1_1_DirectoryMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult<string>> GetData([FromQuery] PaginationParam paginationParams,[FromQuery] DirectoryMaintenance_Param param)
        {
            var data = await _service.Search(paginationParams, param);
            return Ok(data);
        }

        [HttpGet("GetParentDirectoryCode")]
        public async Task<ActionResult<string>> GetParentDirectoryCode()
        {
            var data = await _service.GetParentDirectoryCode();
            return Ok(data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DirectoryMaintenance_Data param)
        {
            param.Update_By = userName;
            param.Update_Time = DateTime.Now;
            var data = await _service.Add(param);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] DirectoryMaintenance_Data param)
        {
            param.Update_By = userName;
            param.Update_Time = DateTime.Now;
            var data = await _service.Update(param);
            return Ok(data);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Detele([FromQuery] string directoryCode)
        {
            var data = await _service.Delete(directoryCode);
            return Ok(data);
        }
    }
}