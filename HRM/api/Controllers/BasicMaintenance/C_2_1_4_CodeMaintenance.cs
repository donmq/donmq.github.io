using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{

    public class C_2_1_4_CodeMaintenance : APIController
    {
        private readonly I_2_1_4_CodeMaintenance _services;
        public C_2_1_4_CodeMaintenance(I_2_1_4_CodeMaintenance services)
        {
            _services = services;
        }



        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam param, [FromQuery] CodeMaintenanceParam filters)
        {
            var result = await _services.GetDataPagination(param, filters);
            return Ok(result);
        }



        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Basic_CodeDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _services.Create(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Basic_CodeDto model)
        {
            model.Update_By = userName; // Open khi có Login
            model.Update_Time = DateTime.Now;
            var result = await _services.Update(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string typeSeq, string code)
        {
            var result = await _services.Delete(typeSeq, code);
            return Ok(result);
        }


        [HttpGet("Export")]
        public async Task<IActionResult> Export([FromQuery] CodeMaintenanceParam filters)
        {
            var result = await _services.ExportExcel(filters);
            return Ok(result);  
        }

        [HttpGet("GetTypeSeqs")]
        public async Task<IActionResult> GetTypeSeqs()
        {
            var result = await _services.GetTypeSeqs();
            return Ok(result);  
        }

    }
}