using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_3_Education : APIController
    {
        private readonly I_4_1_3_Education _services;

        public C_4_1_3_Education(I_4_1_3_Education services)
        {
            _services = services;
        }

        [HttpGet("GetDegrees")]
        public async Task<IActionResult> GetDegrees([FromQuery] string language)
        {
            var result = await _services.GetDegrees(language);
            return Ok(result);
        }

        [HttpGet("GetAcademicSystems")]
        public async Task<IActionResult> GetAcademicSystems([FromQuery] string language)
        {
            var result = await _services.GetAcademicSystems(language);
            return Ok(result);
        }

        [HttpGet("GetMajors")]
        public async Task<IActionResult> GetMajors([FromQuery] string language)
        {
            var result = await _services.GetMajors(language);
            return Ok(result);
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] HRMS_Emp_EducationalParam filter)
        {
            var result = await _services.GetDataPagination(filter);
            return Ok(result);
        }

        [HttpGet("GetEducationFiles")]
        public async Task<IActionResult> GetEducationFiles(string user_GUID)
        {
            var result = await _services.GetEducationFiles(user_GUID);
            return Ok(result);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Emp_EducationalDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _services.Create(model);
            return Ok(result);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Emp_EducationalDto model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _services.Update(model);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Emp_EducationalDto model)
        {
            var result = await _services.Delete(model);
            return Ok(result);
        }

        [HttpDelete("DeleteEducationFile")]
        public async Task<IActionResult> DeleteEducationFile([FromBody] EducationFile model)
        {
            var result = await _services.DeleteEducationFile(model);
            return Ok(result);
        }


        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromForm] EducationUpload model)
        {
            model.UpdateBy = userName;
            var result = await _services.UploadFiles(model);
            return Ok(result);
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromBody] EducationFile model)
        {
            var result = await _services.DownloadFile(model);
            return Ok(result);
        }
    }
}