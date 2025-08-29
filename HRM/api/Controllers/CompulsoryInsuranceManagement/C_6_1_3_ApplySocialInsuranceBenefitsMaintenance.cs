
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
    public class C_6_1_3_ApplySocialInsuranceBenefitsMaintenance : APIController
    {
        private readonly I_6_1_3_ApplySocialInsuranceBenefitsMaintenance _service;
        public C_6_1_3_ApplySocialInsuranceBenefitsMaintenance(I_6_1_3_ApplySocialInsuranceBenefitsMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] ApplySocialInsuranceBenefitsMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(roleList, language));
        }
        [HttpGet("GetListBenefitsKind")]
        public async Task<IActionResult> GetListBenefitsKind(string language)
        {
            return Ok(await _service.GetListBenefitsKind(language));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ApplySocialInsuranceBenefitsMaintenanceDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ApplySocialInsuranceBenefitsMaintenanceDto dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] ApplySocialInsuranceBenefitsMaintenanceDto dto)
        {
            var result = await _service.Delete(dto);
            return Ok(result);
        }

        [HttpPost("Formula")]
        public async Task<IActionResult> Formula([FromBody] ApplySocialInsuranceBenefitsMaintenanceDto dto)
        {
            return Ok(await _service.Formula(dto));
        }
        [HttpPut("GetAdditionData")]
        public async Task<IActionResult> GetAdditionData(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            return Ok(await _service.GetAdditionData(data));
        }
        [HttpGet("GetListTypeHeadEmployeeID")]
        public async Task<IActionResult> GetListTypeHeadEmployeeID(string factory)
        {
            return Ok(await _service.GetListTypeHeadEmployeeID(factory));
        }
        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Download([FromQuery] ApplySocialInsuranceBenefitsMaintenanceParam param)
        {
            var result = await _service.DownloadExcel(param, userName);
            return Ok(result);
        }
        [HttpPost("CheckDate")]
        public async Task<IActionResult> CheckDate([FromBody] ApplySocialInsuranceBenefitsMaintenanceDto dto)
        {
            return Ok(await _service.CheckDate(dto));
        }
    }
}