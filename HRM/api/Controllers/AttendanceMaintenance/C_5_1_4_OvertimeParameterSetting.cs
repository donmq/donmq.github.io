using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_4_OvertimeParameterSetting: APIController
    {
        private readonly I_5_1_4_OvertimeParameterSetting _service;

        public C_5_1_4_OvertimeParameterSetting(I_5_1_4_OvertimeParameterSetting service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Att_Overtime_ParameterParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Att_Overtime_ParameterDTO data)
        {
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Att_Overtime_ParameterDTO data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] HRMS_Att_Overtime_ParameterParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpPost("UploadFileExcel")]
        public async Task<IActionResult> UploadFileExcel([FromForm] HRMS_Att_Overtime_ParameterUploadParam param)
        {
            return Ok(await _service.UploadFileExcel(param, userName));
        }

        [HttpGet("DownloadFileTemplate")]
        public async Task<IActionResult> DownloadFileTemplate()
        {
            var result = await _service.DownloadFileTemplate();
            return Ok(result);
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _service.GetListDivision(language));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string language)
        {
            return Ok(await _service.GetListFactory(division, language, userName));
        }

        [HttpGet("GetListWorkShiftType")]
        public async Task<IActionResult> GetListWorkShiftType(string language)
        {
            return Ok(await _service.GetListWorkShiftType(language));
        }
    }
}