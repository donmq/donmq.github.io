using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_1_EmployeeBasicInformationMaintenance : APIController
    {
        private readonly I_4_1_1_EmployeeBasicInformationMaintenance _service;

        public C_4_1_1_EmployeeBasicInformationMaintenance(I_4_1_1_EmployeeBasicInformationMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetPagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] EmployeeBasicInformationMaintenanceParam param)
        {
            return Ok(await _service.GetPagination(pagination, param, roleList));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string USER_GUID)
        {
            return Ok(await _service.GetDetail(USER_GUID));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeBasicInformationMaintenanceDto dto)
        {
            return Ok(await _service.Add(dto, userName));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] EmployeeBasicInformationMaintenanceDto dto)
        {
            return Ok(await _service.Update(dto, userName));
        }

        [HttpPut("Rehire")]
        public async Task<IActionResult> Rehire([FromBody] EmployeeBasicInformationMaintenanceDto dto)
        {
            return Ok(await _service.Rehire(dto, userName));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string USER_GUID)
        {
            return Ok(await _service.Delete(USER_GUID));
        }

        [HttpGet("DownloadExcelTemplate")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            return Ok(await _service.DownloadExcelTemplate());
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            return Ok(await _service.UploadData(file, roleList, userName));
        }

        [HttpGet("CheckDuplicateCase1")]
        public async Task<IActionResult> CheckDuplicateCase1(string Nationality, string IdentificationNumber)
        {
            return Ok(await _service.CheckDuplicateCase1(Nationality, IdentificationNumber));
        }
        [HttpGet("CheckDuplicateCase2")]
        public async Task<IActionResult> CheckDuplicateCase2([FromQuery] CheckDuplicateParam param)
        {
            return Ok(await _service.CheckDuplicateCase2(param));
        }
        [HttpGet("CheckDuplicateCase3")]
        public async Task<IActionResult> CheckDuplicateCase3([FromQuery] CheckDuplicateParam param)
        {
            return Ok(await _service.CheckDuplicateCase3(param));
        }

        [HttpGet("CheckBlackList")]
        public async Task<IActionResult> CheckBlackList([FromQuery] CheckBlackList param)
        {
            return Ok(await _service.CheckBlackList(param));
        }

        #region Get List
        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision([FromQuery] string Language)
        {
            return Ok(await _service.GetListDivision(Language));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string Division, [FromQuery] string Language)
        {
            return Ok(await _service.GetListFactory(Division, Language));
        }
        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality([FromQuery] string Language)
        {
            return Ok(await _service.GetListNationality(Language));
        }
        [HttpGet("GetListPermission")]
        public async Task<IActionResult> GetListPermission([FromQuery] string Language)
        {
            return Ok(await _service.GetListPermission(Language));
        }
        [HttpGet("GetListIdentityType")]
        public async Task<IActionResult> GetListIdentityType([FromQuery] string Language)
        {
            return Ok(await _service.GetListIdentityType(Language));
        }
        [HttpGet("GetListEducation")]
        public async Task<IActionResult> GetListEducation([FromQuery] string Language)
        {
            return Ok(await _service.GetListEducation(Language));
        }
        [HttpGet("GetListWorkType")]
        public async Task<IActionResult> GetListWorkType([FromQuery] string Language)
        {
            return Ok(await _service.GetListWorkType(Language));
        }
        [HttpGet("GetListRestaurant")]
        public async Task<IActionResult> GetListRestaurant([FromQuery] string Language)
        {
            return Ok(await _service.GetListRestaurant(Language));
        }
        [HttpGet("GetListReligion")]
        public async Task<IActionResult> GetListReligion([FromQuery] string Language)
        {
            return Ok(await _service.GetListReligion(Language));
        }
        [HttpGet("GetListTransportationMethod")]
        public async Task<IActionResult> GetListTransportationMethod([FromQuery] string Language)
        {
            return Ok(await _service.GetListTransportationMethod(Language));
        }
        [HttpGet("GetListVehicleType")]
        public async Task<IActionResult> GetListVehicleType([FromQuery] string Language)
        {
            return Ok(await _service.GetListVehicleType(Language));
        }
        [HttpGet("GetListWorkLocation")]
        public async Task<IActionResult> GetListWorkLocation([FromQuery] string Language)
        {
            return Ok(await _service.GetListWorkLocation(Language));
        }
        [HttpGet("GetListReasonResignation")]
        public async Task<IActionResult> GetListReasonResignation([FromQuery] string Language)
        {
            return Ok(await _service.GetListReasonResignation(Language));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string Division, string Factory, string Language)
        {
            return Ok(await _service.GetListDepartment(Division, Factory, Language));
        }
        [HttpGet("GetListProvinceDirectly")]
        public async Task<IActionResult> GetListProvinceDirectly(string char1, string Language)
        {
            return Ok(await _service.GetListProvinceDirectly(char1, Language));
        }
        [HttpGet("GetListCity")]
        public async Task<IActionResult> GetListCity(string char1, string Language)
        {
            return Ok(await _service.GetListCity(char1, Language));
        }

        [HttpGet("GetPositionGrade")]
        public async Task<IActionResult> GetPositionGrade()
        {
            return Ok(await _service.GetPositionGrade());
        }
        [HttpGet("GetPositionTitle")]
        public async Task<IActionResult> GetPositionTitle(decimal level, string Language)
        {
            return Ok(await _service.GetPositionTitle(level, Language));
        }

        [HttpGet("GetDepartmentSupervisor")]
        public async Task<IActionResult> GetDepartmentSupervisor(string USER_GUID, string Language)
        {
            return Ok(await _service.GetDepartmentSupervisor(USER_GUID, Language));
        }
        [HttpGet("GetListWorkTypeShift")]
        public async Task<IActionResult> GetListWorkTypeShift([FromQuery] string Language)
        {
            return Ok(await _service.GetListWorkTypeShift(Language));
        }
        #endregion
    }
}