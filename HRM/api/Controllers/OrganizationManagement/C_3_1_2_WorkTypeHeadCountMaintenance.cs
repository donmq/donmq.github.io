using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizationManagement
{

    public class C_3_1_2_WorkTypeHeadCountMaintenance : APIController
    {
        private readonly I_3_1_2_WorkTypeHeadCountMaintenance _services;
        public C_3_1_2_WorkTypeHeadCountMaintenance(I_3_1_2_WorkTypeHeadCountMaintenance services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam param, [FromQuery] HRMS_Org_Work_Type_HeadcountParam filter)
        {
            var result = await _services.GetDataPagination(param, filter);
            return Ok(result);
        }

        [HttpGet("GetDivisions")]
        public async Task<IActionResult> GetDivisions(string language)
        {
            var result = await _services.GetDivisions(language);
            return Ok(result);
        }

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetFactories(language);
            return Ok(result);
        }

        [HttpGet("GetFactoriesByDivision")]
        public async Task<IActionResult> GetFactoriesByDivision(string division, string language)
        {
            var result = await _services.GetFactories(division, language);
            return Ok(result);
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments(string language)
        {
            var result = await _services.GetDepartments(language);
            return Ok(result);
        }

        [HttpGet("GetDepartmentsByDivisionFactory")]
        public async Task<IActionResult> GetDepartmentsByDivisionFactory(string division, string factory, string language)
        {
            var result = await _services.GetDepartments(division, factory, language);
            return Ok(result);
        }

        [HttpGet("GetDepartmentName")]
        public async Task<IActionResult> GetDepartmentName([FromQuery] HRMS_Org_Work_Type_HeadcountParam param)
        {
            var result = await _services.GetDepartmentNameFromDepartmentCode(param);
            return Ok(result);
        }

        [HttpGet("GetWorkCodeNames")]
        public async Task<IActionResult> GetWorkCodeNames()
        {
            var result = await _services.GetWorkCodeNames();
            return Ok(result);
        }

        [HttpGet("GetListUpdate")]
        public async Task<IActionResult> GetListUpdate([FromQuery] HRMS_Org_Work_Type_HeadcountParam filter)
        {
            var result = await _services.GetListUpdate(filter);
            return Ok(result);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<HRMS_Org_Work_Type_HeadcountDto> model)
        {

            var result = await _services.Create(model, userName);
            return Ok(result);
        }



        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] HRMS_Org_Work_Type_HeadcountUpdate model)
        {
            var result = await _services.Update(model, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Org_Work_Type_HeadcountDto model)
        {
            var result = await _services.Delete(model);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] HRMS_Org_Work_Type_HeadcountParam param)
        {
            var result = await _services.DownloadExcel(param);
            return Ok(result);
        }

    }
}