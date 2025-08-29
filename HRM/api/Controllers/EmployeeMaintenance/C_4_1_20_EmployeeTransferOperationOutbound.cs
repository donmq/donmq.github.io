using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_20_EmployeeTransferOperationOutbound : APIController
    {
        private readonly I_4_1_20_EmployeeTransferOperationOutbound _service;

        public C_4_1_20_EmployeeTransferOperationOutbound(I_4_1_20_EmployeeTransferOperationOutbound service)
        {
            _service = service;
        }

        [HttpGet("GetPagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] EmployeeTransferOperationOutboundParam param)
        {
            return Ok(await _service.GetPagination(pagination, param, roleList));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string History_GUID)
        {
            return Ok(await _service.GetDetail(History_GUID));
        }

        [HttpGet("GetEmployeeInformation")]
        public async Task<IActionResult> GetEmployeeInformation([FromQuery] EmployeeInformationParam param)
        {
            return Ok(await _service.GetEmployeeInformation(param));
        }

        [HttpGet("GetEmployeeID")]
        public async Task<IActionResult> GetEmployeeID()
        {
            return Ok(await _service.GetEmployeeID());
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeTransferOperationOutboundDto dto)
        {
            return Ok(await _service.Add(dto, userName));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] EmployeeTransferOperationOutboundDto dto)
        {
            return Ok(await _service.Update(dto, userName));
        }

        #region Get List
        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision([FromQuery] string Language)
        {
            return Ok(await _service.GetListDivision(Language));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory( [FromQuery] string Language)
        {
            return Ok(await _service.GetListFactory(Language));
        }
        [HttpGet("GetListFactoryByDivision")]
        public async Task<IActionResult> GetListFactoryByDivision([FromQuery] string Division, [FromQuery] string Language)
        {
            return Ok(await _service.GetListFactoryByDivision(Division, Language));
        }
        [HttpGet("GetListNationality")]
        public async Task<IActionResult> GetListNationality([FromQuery] string Language)
        {
            return Ok(await _service.GetListNationality(Language));
        }
        [HttpGet("GetListWorkType")]
        public async Task<IActionResult> GetListWorkType([FromQuery] string Language)
        {
            return Ok(await _service.GetListWorkType(Language));
        }

        [HttpGet("GetListReasonChangeOut")]
        public async Task<IActionResult> GetListReasonChangeOut([FromQuery] string Language)
        {
            return Ok(await _service.GetListReasonChangeOut(Language));
        }

        [HttpGet("GetListReasonChangeIn")]
        public async Task<IActionResult> GetListReasonChangeIn([FromQuery] string Language)
        {
            return Ok(await _service.GetListReasonChangeIn(Language));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string Division, string Factory, string Language)
        {
            return Ok(await _service.GetListDepartment(Division, Factory, Language));
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
        #endregion
    }
}