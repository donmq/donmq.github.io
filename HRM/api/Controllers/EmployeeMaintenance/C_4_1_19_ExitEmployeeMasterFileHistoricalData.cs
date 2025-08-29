using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_19_ExitEmployeeMasterFileHistoricalData : APIController
    {
        private readonly I_4_1_19_ExitEmployeeMasterFileHistoricalData _service;

        public C_4_1_19_ExitEmployeeMasterFileHistoricalData(I_4_1_19_ExitEmployeeMasterFileHistoricalData service)
        {
            _service = service;
        }

        [HttpGet("GetPagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] ExitEmployeeMasterFileHistoricalDataParam param)
        {
            return Ok(await _service.GetPagination(pagination, param, userName));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] string USER_GUID, [FromQuery] string Resign_Date)
        {
            return Ok(await _service.GetDetail(USER_GUID, Resign_Date));
        }

        #region Get List
        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality([FromQuery] string Language)
        {
            return Ok(await _service.GetListNationality(Language));
        }

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

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string Division, string Factory, string Language)
        {
            return Ok(await _service.GetListDepartment(Division, Factory, Language));
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

        [HttpGet("GetListWorkTypeShift")]
        public async Task<IActionResult> GetListWorkTypeShift([FromQuery] string Language)
        {
            return Ok(await _service.GetListWorkTypeShift(Language));
        }
        #endregion
    }
}