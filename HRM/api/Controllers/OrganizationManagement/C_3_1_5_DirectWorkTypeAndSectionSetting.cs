using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizationManagement
{
    public class C_3_1_5_DirectWorkTypeAndSectionSetting : APIController
    {
        private readonly I_3_1_5_DirectWorkTypeAndSectionSetting _service;
        public C_3_1_5_DirectWorkTypeAndSectionSetting(I_3_1_5_DirectWorkTypeAndSectionSetting service)
        {
            _service = service;
        }
        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] DirectWorkTypeAndSectionSettingParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HRMS_Org_Direct_SectionDto data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Org_Direct_SectionDto data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Download([FromQuery] DirectWorkTypeAndSectionSettingParam param)
        {
            var result = await _service.DownloadFileExcel(param);
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
            return Ok(await _service.GetListFactory(division, language));
        }

        [HttpGet("GetListWorkType")]
        public async Task<IActionResult> GetListWorkType(string language)
        {
            return Ok(await _service.GetListWorkType(language));
        }

        [HttpGet("GetListSection")]
        public async Task<IActionResult> GetListSection(string language)
        {
            return Ok(await _service.GetListSection(language));
        }
        [HttpGet("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate([FromQuery] DirectWorkTypeAndSectionSettingParam param)
        {
            var result = await _service.CheckDuplicate(param);
            return Ok(result);
        }
    }
}