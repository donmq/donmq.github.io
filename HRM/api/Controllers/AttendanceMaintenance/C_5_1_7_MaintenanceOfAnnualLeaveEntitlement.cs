using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_7_MaintenanceOfAnnualLeaveEntitlement : APIController
    {
        private readonly I_5_1_7_MaintenanceOfAnnualLeaveEntitlement _service;

        public C_5_1_7_MaintenanceOfAnnualLeaveEntitlement(I_5_1_7_MaintenanceOfAnnualLeaveEntitlement service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] MaintenanceOfAnnualLeaveEntitlementDto dto)
            => Ok(await _service.Add(dto, userName));

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] MaintenanceOfAnnualLeaveEntitlementDto dto)
        {
            dto.Update_By = userName;
            return Ok(await _service.Edit(dto));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] MaintenanceOfAnnualLeaveEntitlementDto dto)
            => Ok(await _service.Delete(dto));

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _service.GetListFactory(userName, language));

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _service.GetListDepartment(factory, language));

        [HttpGet("GetListLeaveCode")]
        public async Task<IActionResult> GetListLeaveCode([FromQuery] string language)
            => Ok(await _service.GetListLeaveCode(language));

        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] PaginationParam pagination, [FromQuery] MaintenanceOfAnnualLeaveEntitlementParam param)
            => Ok(await _service.Query(pagination, param));

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            var result = await _service.ExportExcel();
            return File(result.FileContents, "application/xlsx");
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] MaintenanceOfAnnualLeaveEntitlementParam param)
        {
            var result = await _service.DownloadExcel(param, userName);
            return Ok(result);
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file, [FromForm] string language)
        {
            UploadFormData formData = new()
            {
                File = file,
                Language = language,
                UserName = userName,
                UserRoles = roleList
            };
            return Ok(await _service.UploadExcel(formData));
        }
        [HttpGet("CheckExistedData")]
        public async Task<IActionResult> CheckExistedData(string Annual_Start, string Factory, string Employee_ID, string Leave_Code)
        {
            return Ok(await _service.CheckExistedData(Annual_Start, Factory, Employee_ID, Leave_Code));
        }
    }
}