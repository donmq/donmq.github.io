using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.OrganizationManagement
{

    public class C_3_1_4_DirectDepartmentSetting : APIController
    {
        private readonly I_3_1_4_DirectDepartmentSetting _services;
        public C_3_1_4_DirectDepartmentSetting(I_3_1_4_DirectDepartmentSetting services)
        {
            _services = services;
        }
        [HttpGet("getData")]
        public async Task<IActionResult> Getdata([FromQuery] PaginationParam pagination, [FromQuery] Org_Direct_DepartmentParam param)
        {
            var result = await _services.Getdata(pagination, param);
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] List<Org_Direct_DepartmentParamQuery> model)
        {
            var result = await _services.AddNew(model, userName);
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] List<Org_Direct_DepartmentParamQuery> model)
        {
            var result = await _services.Edit(model, userName);
            return Ok(result);
        }
        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string Language)
        {
            return Ok(await _services.GetListDivision(Language));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string Division, string Language)
        {
            return Ok(await _services.GetListFactory(Division, Language));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string Division, string Factory, string Language)
        {
            return Ok(await _services.GetListDepartment(Language));
        }
        [HttpGet("GetListLine")]
        public async Task<IActionResult> GetListLine(string Division, string Factory)
        {
            return Ok(await _services.GetListLine(Division, Factory));
        }
        [HttpGet("GetListDirectDepartmentAttribute")]
        public async Task<IActionResult> GetListDirectDepartmentAttribute()
        {
            return Ok(await _services.GetListDirectDepartmentAttribute());
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Org_Direct_DepartmentParamQuery model)
        {
            var result = await _services.Delete(model);
            return Ok(result);
        }
        [HttpGet("Getdetail")]
        public async Task<IActionResult> Getdetail([FromQuery] Org_Direct_DepartmentParam model)
        {
            return Ok(await _services.Getdetail(model));
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] Org_Direct_DepartmentParam param)
        {
            var result = await _services.DownloadExcel(param);
            return Ok(result);
        }
        [HttpPut("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate([FromBody] List<Org_Direct_DepartmentResult> model)
        {
            var result = await _services.CheckDuplicate(model);
            return Ok(result);
        }
    }
}