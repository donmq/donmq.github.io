
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_18_SalaryAdjustmentMaintenanceController : APIController
    {
        private readonly I_7_1_18_SalaryAdjustmentMaintenance _services;

        public C_7_1_18_SalaryAdjustmentMaintenanceController(I_7_1_18_SalaryAdjustmentMaintenance services)
        {
            _services = services;
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] SalaryAdjustmentMaintenanceParam param)
        {
            var result = await _services.GetDataPagination(pagination, param);
            return Ok(result);
        }
        [HttpGet("GetDetailPersonal")]
        public async Task<IActionResult> GetDetailPersonal(string factory, string employee_ID, string language)
        {
            var result = await _services.GetDetailPersonal(factory, employee_ID, language);
            return Ok(result);
        }
        [HttpGet("CheckEffectiveDate")]
        public async Task<IActionResult> CheckEffectiveDate(string factory, string employee_ID, string inputEffectiveDate)
        {
            var result = await _services.CheckEffectiveDate(factory, employee_ID, inputEffectiveDate);
            return Ok(result);
        }
        [HttpGet("GetListEmployeeID")]
        public async Task<IActionResult> GetListEmployeeID(string factory)
        {
            var result = await _services.GetListEmployeeID(factory);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SalaryAdjustmentMaintenanceMain data)
        {
            data.Update_By = userName;
            var result = await _services.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SalaryAdjustmentMaintenanceMain data)
        {
            data.Update_By = userName;
            var result = await _services.Update(data);
            return Ok(result);
        }
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _services.UploadExcel(file, roleList, userName);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] SalaryAdjustmentMaintenanceParam param)
        {
            var result = await _services.DownloadExcel(param, userName);
            return Ok(result);
        }

        [HttpGet("DownloadTemplate")]
        public async Task<ActionResult> DownloadTemplate(string factory)
        {
            var result = await _services.DownloadTemplate(factory);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            var result = await _services.GetListFactory(language, roleList);
            return Ok(result);
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string factory, string language)
        {
            var result = await _services.GetListDepartment(factory, language);
            return Ok(result);
        }
        [HttpGet("GetListReason")]
        public async Task<IActionResult> GetListReason(string language)
        {
            var result = await _services.GetListReason(language);
            return Ok(result);
        }
        [HttpGet("GetListTechnicalType")]
        public async Task<IActionResult> GetListTechnicalType(string language)
        {
            var result = await _services.GetListTechnicalType(language);
            return Ok(result);
        }
        [HttpGet("GetListExpertiseCategory")]
        public async Task<IActionResult> GetListExpertiseCategory(string language)
        {
            var result = await _services.GetListExpertiseCategory(language);
            return Ok(result);
        }
        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string language)
        {
            var result = await _services.GetListPermissionGroup(language);
            return Ok(result);
        }
        [HttpGet("GetListSalaryType")]
        public async Task<IActionResult> GetListSalaryType(string language)
        {
            var result = await _services.GetListSalaryType(language);
            return Ok(result);
        }
        [HttpGet("GetListSalaryItem")]
        public async Task<IActionResult> GetListSalaryItem(string history_GUID, string language)
        {
            var result = await _services.GetListSalaryItem(history_GUID, language);
            return Ok(result);
        }
        [HttpGet("GetListPositionTitle")]
        public async Task<IActionResult> GetListPositionTitle(string language)
        {
            var result = await _services.GetListPositionTitle(language);
            return Ok(result);
        }
        [HttpGet("GetListCurrency")]
        public async Task<IActionResult> GetListCurrency(string language)
        {
            var result = await _services.GetListCurrency(language);
            return Ok(result);
        }
        [HttpGet("GetSalaryItemsAsync")]
        public async Task<IActionResult> GetSalaryItemsAsync(string factory, string permissionGroup, string salaryType, string type, string language, string employeeID)
        {
            var result = await _services.GetSalaryItemsAsync(factory, permissionGroup, salaryType, type, language, employeeID);
            return Ok(result);
        }
        [HttpGet("CheckReasonForChange")]
        public async Task<IActionResult> CheckReasonForChange(string reasonForChange)
        {
            var result = await _services.CheckReasonForChange(reasonForChange);
            return Ok(result);
        }
    }
}