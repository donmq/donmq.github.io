using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizationManagement
{
    public class C_3_1_1_DepartmentMaintenance : APIController
    {
        private readonly I_3_1_1_DepartmentMaintenance _service;

        public C_3_1_1_DepartmentMaintenance(I_3_1_1_DepartmentMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] HRMS_Org_Department_Param param)
        {
            var data = await _service.GetDataPagination(pagination, param);
            return Ok(data);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] HRMS_Org_Department model)
        {
            model.Update_Time = DateTime.Now;
            var result = await _service.Add(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] HRMS_Org_Department model)
        {
            model.Update_By = userName;
            model.Update_Time = DateTime.Now;
            var result = await _service.Update(model);
            return Ok(result);
        }

        [HttpGet("GetLanguage")]
        public async Task<IActionResult> GetLanguage()
        {
            return Ok(await _service.GetLanguage());
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string departmentCode , string division , string factory)
        {
            return Ok(await _service.GetDetail(departmentCode, division, factory));
        }

        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] HRMS_Org_Department_Param param)
        {
            var result = await _service.DownloadExcel(param);
            return Ok(result);            
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string lang)
        {
            return Ok(await _service.GetListDepartment(division, factory, lang));
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string lang)
        {
            return Ok(await _service.GetListDivision(lang));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string lang)
        {
            return Ok(await _service.GetListFactory(division, lang));
        }

        [HttpGet("GetListLevel")]
        public async Task<IActionResult> GetListLevel(string lang)
        {
            return Ok(await _service.GetListLevel(lang));
        }

        [HttpGet("GetListUpperVirtual")]
        public async Task<IActionResult> GetListUpperVirtual(string departmentCode, string division, string factory, string lang)
        {
            return Ok(await _service.GetListUpperVirtual(departmentCode, division, factory, lang));
        }

        [HttpGet("CheckListDeptCode")]
        public async Task<IActionResult> CheckListDeptCode(string division, string factory, string deptCode )
        {
            return Ok(await _service.CheckListDeptCode(division, factory, deptCode));
        }

        [HttpPost("AddLanguage")]
        public async Task<IActionResult> AddLanguage([FromBody] LanguageDeparment model)
        {
            model.userName = userName;
            var result = await _service.AddLanguage(model);
            return Ok(result);
        }

        [HttpPut("UpdateLanguage")]
        public async Task<ActionResult> UpdateLanguage([FromBody] LanguageDeparment model)
        {
            model.userName = userName;
            var result = await _service.UpdateLanguage(model);
            return Ok(result);
        }
    }
}