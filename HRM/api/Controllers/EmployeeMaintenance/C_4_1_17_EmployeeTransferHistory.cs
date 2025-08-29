using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_17_EmployeeTransferHistory : APIController
    {
        private readonly I_4_1_17_EmployeeTransferHistory _service;
        public C_4_1_17_EmployeeTransferHistory(I_4_1_17_EmployeeTransferHistory service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] EmployeeTransferHistoryParam param)
        {
            var result = await _service.GetDataPagination(pagination, param, roleList);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Download([FromQuery] EmployeeTransferHistoryParam param)
        {
            var result = await _service.DownloadFileExcel(param, roleList);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeTransferHistoryDTO data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] EmployeeTransferHistoryDetele data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }

        [HttpDelete("BatchDelete")]
        public async Task<IActionResult> BatchDelete(List<EmployeeTransferHistoryDetele> data)
        {
            var result = await _service.BatchDelete(data);
            return Ok(result);
        }

        [HttpPut("EffectiveConfirm")]
        public async Task<IActionResult> EffectiveConfirm(List<EmployeeTransferHistoryEffectiveConfirm> data)
        {
            var result = await _service.EffectiveConfirm(data, userName);
            return Ok(result);
        }
        [HttpGet("CheckEffectiveConfirm")]
        public async Task<IActionResult> CheckEffectiveConfirm([FromQuery] EmployeeTransferHistoryEffectiveConfirm data)
        {
            var result = await _service.CheckEffectiveConfirm(data);
            return Ok(result);
        }
        [HttpGet("GetDataDetail")]
        public async Task<IActionResult> GetDataDetail(string division, string employee_ID, string factory)
        {
            return Ok(await _service.GetDataDetail(division, employee_ID, factory));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeTransferHistoryDTO data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string language)
        {
            return Ok(await _service.GetListDivision(language));
        }


        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language, string division)
        {
            return Ok(await _service.GetListFactory(language, division));
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory, string division)
        {
            return Ok(await _service.GetListDepartment(language, factory, division));
        }

        [HttpGet("GetListAssignedDivisionAfter")]
        public async Task<IActionResult> GetListAssignedDivisionAfter(string language)
        {
            return Ok(await _service.GetListAssignedDivisionAfter(language));
        }
        [HttpGet("GetListAssignedFactoryAfter")]
        public async Task<IActionResult> GetListAssignedFactoryAfter(string language, string assignedDivisionAfter)
        {
            return Ok(await _service.GetListAssignedFactoryAfter(language, assignedDivisionAfter));
        }
        [HttpGet("GetListDepartmentAfter")]
        public async Task<IActionResult> GetListDepartmentAfter(string language, string assignedDivisionAfter, string assignedFactoryAfter)
        {
            return Ok(await _service.GetListDepartmentAfter(language, assignedDivisionAfter, assignedFactoryAfter));
        }

        [HttpGet("GetListPositionGrade")]
        public async Task<IActionResult> GetListPositionGrade(string language)
        {
            return Ok(await _service.GetListPositionGrade(language));
        }

        [HttpGet("GetListPositionTitle")]
        public async Task<IActionResult> GetListPositionTitle(string language, decimal? positionGrade)
        {
            return Ok(await _service.GetListPositionTitle(language, positionGrade));
        }

        [HttpGet("GetListWorkType")]
        public async Task<IActionResult> GetListWorkType(string language)
        {
            return Ok(await _service.GetListWorkType(language));
        }

        [HttpGet("GetListReasonforChange")]
        public async Task<IActionResult> GetListReasonforChange(string language)
        {
            return Ok(await _service.GetListReasonforChange(language));
        }

        [HttpGet("GetListTypeHeadEmployeeID")]
        public async Task<IActionResult> GetListTypeHeadEmployeeID(string factory, string division)
        {
            return Ok(await _service.GetListTypeHeadEmployeeID(factory, division));
        }

        [HttpGet("GetListDataSource")]
        public async Task<IActionResult> GetListDataSource(string language)
        {
            return Ok(await _service.GetListDataSource(language));
        }
    }
}