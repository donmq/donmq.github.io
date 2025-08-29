using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_2_EmployeeEmergencyContacts : APIController
    {
        private readonly I_4_1_2_EmployeeEmergencyContacts _service;
        public C_4_1_2_EmployeeEmergencyContacts(I_4_1_2_EmployeeEmergencyContacts service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] EmployeeEmergencyContactsParam param)
        {
            var result = await _service.GetData(param);
            return Ok(result);
        }

        [HttpGet("GetRelationships")]
        public async Task<IActionResult> GetRelationships([FromQuery] string Language)
        {
            var result = await _service.GetRelationships(Language);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeEmergencyContactsDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Create(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeEmergencyContactsDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Update(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] EmployeeEmergencyContactsDto model)
        {
            var result = await _service.Delete(model);
            return Ok(result);
        }

        [HttpGet("DownloadExcelTemplate")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            return Ok(await _service.DownloadExcelTemplate());
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            return Ok(await _service.UploadData(file, roleList, userName));
        }

        [HttpGet("GetSeqMax")]
        public async Task<IActionResult> GetSeqMax(string USER_GUID)
        {
            var result = await _service.GetMaxSeq(USER_GUID);
            return Ok(result);
        }
    }
}