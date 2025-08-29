using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_21_EmployeeTransferOperationInbound : APIController
    {
        private readonly I_4_1_21_EmployeeTransferOperationInbound _service;

        public C_4_1_21_EmployeeTransferOperationInbound(I_4_1_21_EmployeeTransferOperationInbound service)
        {
            _service = service;
        }

        [HttpGet("GetPagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParam pagination, [FromQuery] EmployeeTransferOperationInboundParam param)
        {
            return Ok(await _service.GetPagination(pagination, param, roleList));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string History_GUID)
        {
            return Ok(await _service.GetDetail(History_GUID));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] EmployeeTransferOperationInboundDto dto)
        {
            return Ok(await _service.Update(dto, userName));
        }

        [HttpPut("Confirm")]
        public async Task<IActionResult> Confirm([FromBody] EmployeeTransferOperationInboundDto dto)
        {
            return Ok(await _service.Confirm(dto, userName));
        }

        #region Get List
        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision([FromQuery] string Language)
        {
            return Ok(await _service.GetListDivision(Language));
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string Language)
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

        [HttpGet("GetListReasonChange")]
        public async Task<IActionResult> GetListReasonChange([FromQuery] string Language)
        {
            return Ok(await _service.GetListReasonChange(Language));
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