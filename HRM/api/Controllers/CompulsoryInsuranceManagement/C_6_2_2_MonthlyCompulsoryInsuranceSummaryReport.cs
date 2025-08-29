using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
    public class C_6_2_2_MonthlyCompulsoryInsuranceSummaryReport : APIController
    {
        private readonly I_6_2_2_MonthlyCompulsoryInsuranceSummaryReport _service;
        
        public C_6_2_2_MonthlyCompulsoryInsuranceSummaryReport(I_6_2_2_MonthlyCompulsoryInsuranceSummaryReport service)
        {
            _service = service;
        }

        [HttpGet("GetCountRecords")]
        public async Task<IActionResult> GetCountRecords([FromQuery] MonthlyCompulsoryInsuranceSummaryReport_Param param)
        {
            var data = await _service.GetCountRecords(param);
            return Ok(data);
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] MonthlyCompulsoryInsuranceSummaryReport_Param param)
        {
            var data = await _service.DownLoadExcel(param, userName);
            return Ok(data);
        }

        [HttpGet("GetFactoryList")]
        public async Task<IActionResult> GetFactoryList([FromQuery] string language)
        {
            var data = await _service.GetListFactory(language, userName);
            return Ok(data);
        }

        [HttpGet("GetDepartmentList")]
        public async Task<IActionResult> GetDepartmentList([FromQuery] string factory, [FromQuery] string language)
        {
            var data = await _service.GetListDepartment(factory, language);
            return Ok(data);
        }

        [HttpGet("GetPermissionGroup")]
        public async Task<IActionResult> GetPermissionGroup([FromQuery] string factory,[FromQuery] string language)
        {
            var data = await _service.GetListPermissionGroup(factory, language);
            return Ok(data);
        }

        [HttpGet("GetListInsuranceType")]
        public async Task<IActionResult> GetListInsuranceType([FromQuery] string language)
        {
            var data = await _service.GetListInsuranceType(language);
            return Ok(data);
        }

    }
}