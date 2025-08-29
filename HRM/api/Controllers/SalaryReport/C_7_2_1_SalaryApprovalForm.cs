using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_2_1_SalaryApprovalForm  : APIController
    {
        private readonly I_7_2_1_SalaryApprovalForm _service;
        public C_7_2_1_SalaryApprovalForm(I_7_2_1_SalaryApprovalForm service)
        {
            _service = service;
        }
        [HttpGet("GetSearch")]
        public async Task<IActionResult> Search([FromQuery] D_7_2_1_SalaryApprovalForm_Param param)
        {
            return Ok(await _service.Search(param));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _service.GetListFactory(language, userName);
            return Ok(result);
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language, factory));
        }
        [HttpGet("GetPositionTitles")]
        public async Task<IActionResult> GetPositionTitles(string language)
        {
            return Ok(await _service.GetPositionTitles(language));
        }
        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult>GetListPermissionGroup(string factory, string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }
        [HttpGet("ExportPDF")]
        public async Task<IActionResult>ExportPDF([FromQuery] D_7_2_1_SalaryApprovalForm_Param param)
        {
            return Ok(await _service.ExportPDF(param, userName));
        }
    }
}