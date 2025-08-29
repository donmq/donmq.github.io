using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_6_EmployeeLucnhBreakTimeSetting : APIController
    {
        private readonly I_5_1_6_EmployeeLucnhBreakTimeSetting _service;

        public C_5_1_6_EmployeeLucnhBreakTimeSetting(I_5_1_6_EmployeeLucnhBreakTimeSetting service)
        {
            _service = service;
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] EmployeeLunchBreakTimeSettingParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param, roleList));
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            return Ok(await _service.UploadData(file, roleList, userName));
        }

        [HttpGet("DownloadExcelTemplate")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            return Ok(await _service.DownloadExcelTemplate());
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Att_LunchtimeDto data)
        {
            return Ok(await _service.Delete(data));
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] EmployeeLunchBreakTimeSettingParam param)
        {
            var result = await _service.DownloadExcel(param, roleList, userName);
            return Ok(result);
        }
    }
}