using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Services.Interfaces.RewardandPenaltyMaintenance;
using API.DTOs.RewardandPenaltyMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RewardandPenaltyMaintenance
{
    public class C_8_1_2_EmployeeRewardPenaltyRecords : APIController
    {
        private readonly I_8_1_2_EmployeeRewardPenaltyRecords _service;
        public C_8_1_2_EmployeeRewardPenaltyRecords(I_8_1_2_EmployeeRewardPenaltyRecords service)
        {
            _service = service;
        }

        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList([FromQuery] D_8_1_2_EmployeeRewardPenaltyRecordsParam param)
        {
            var result = await _service.GetEmployeeList(param);
            return Ok(result);
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            var result = await _service.GetListDepartment(language, factory);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(userName, language);
            return Ok(result);
        }

        [HttpGet("GetListRewardType")]
        public async Task<IActionResult> GetListRewardType(string language)
        {
            var result = await _service.GetListRewardType(language);
            return Ok(result);
        }

        [HttpGet("GetListReasonCode")]
        public async Task<IActionResult> GetListReasonCode(string factory)
        {
            var result = await _service.GetListReasonCode(factory);
            return Ok(result);
        }

        [HttpGet("GetSearch")]
        public async Task<IActionResult> GetSearch([FromQuery] PaginationParam pagination, [FromQuery] D_8_1_2_EmployeeRewardPenaltyRecordsParam searchParam)
        {
            return Ok(await _service.GetSearch(pagination, searchParam));
        }

        [HttpGet("Data_Detail")]
        public async Task<IActionResult> Data_DetailData_Detail([FromQuery] string History_GUID, string Language)
        {
            var result = await _service.Data_Detail(History_GUID, Language);
            return Ok(result);
        }

        [HttpPost("DownloadFile")]
        public async Task<ActionResult> DownloadFile([FromBody] EmployeeRewardPenaltyRecordsReportDownloadFileModel param)
        {
            var result = await _service.DownloadFile(param);
            return Ok(result);
        }

        [HttpGet("DownloadTemplate")]
        public async Task<IActionResult> DownloadTemplate()
        {
            var result = await _service.DownloadTemplate();
            return Ok(result);
        }

        [HttpPost("UploadFileExcel")]
        public async Task<IActionResult> UploadFileExcel(IFormFile file)
        {
            var result = await _service.UploadFileExcel(file, roleList, userName);
            return Ok(result);
        }

        [HttpPut("PutData")]
        public async Task<IActionResult> Update([FromBody] D_8_1_2_EmployeeRewardPenaltyRecordsSubParam data)
        {
            var result = await _service.Update(data, userName);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] D_8_1_2_EmployeeRewardPenaltyRecordsSubParam data)
        {
            var result = await _service.Create(data, userName);
            return Ok(result);
        }

        [HttpDelete("DeleteData")]
        public async Task<IActionResult> DeleteData(D_8_1_2_EmployeeRewardPenaltyRecordsData data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }
        
    }
}