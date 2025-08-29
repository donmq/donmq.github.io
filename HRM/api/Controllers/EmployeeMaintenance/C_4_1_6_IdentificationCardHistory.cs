using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_6_IdentificationCardHistory : APIController
    {
        private readonly I_4_1_6_IdentificationCardHistory _service;
        public C_4_1_6_IdentificationCardHistory(I_4_1_6_IdentificationCardHistory service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] HRMS_Emp_Identity_Card_HistoryParam param)
        {
            var result = await _service.GetData(param);
            return Ok(result);
        }
       
        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality([FromQuery] string Language)
        {
            return Ok(await _service.GetListNationality(Language));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Emp_Identity_Card_HistoryDto data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }
        [HttpGet("GetListTypeHeadIdentificationNumber")]
        public async Task<IActionResult> GetListTypeHeadIdentificationNumber(string nationality)
        {
            return Ok(await _service.GetListTypeHeadIdentificationNumber(nationality));
        }
    }
}