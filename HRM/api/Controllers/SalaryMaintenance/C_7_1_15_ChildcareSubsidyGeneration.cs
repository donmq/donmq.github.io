using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_15_ChildcareSubsidyGenerationController : APIController
    {
        private readonly I_7_1_15_ChildcareSubsidyGeneration _services;

        public C_7_1_15_ChildcareSubsidyGenerationController(I_7_1_15_ChildcareSubsidyGeneration services)
        {
            _services = services;
        }


        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser([FromQuery] string language)
        {
            var result = await _services.GetListFactoryByUser(userName, language);
            return Ok(result);
        }

        [HttpGet("GetListPermissionGroupByFactory")]
        public async Task<IActionResult> GetListPermissionGroupByFactory([FromQuery] string factory, [FromQuery] string language)
        {
            var result = await _services.GetListPermissionGroupByFactory(factory, language);
            return Ok(result);
        }

        [HttpGet("CheckParam")]
        public async Task<IActionResult> CheckParam([FromQuery] ChildcareSubsidyGenerationParam param)
        {
            return Ok(await _services.CheckParam(param));
        }

        [HttpGet("GetTotalTab2")]
        public async Task<IActionResult> GetTotalTab2([FromQuery] ChildcareSubsidyGenerationParam param)
        {
            return Ok(await _services.GetTotaTab2(param));
        }      

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] ChildcareSubsidyGenerationParam param)
        {
            return Ok(await _services.ExcuteTab2(param, userName));
        }

        [HttpGet("InsertData")]
        public async Task<IActionResult> InsertData([FromQuery] ChildcareSubsidyGenerationParam param)
        {
            return Ok(await _services.ExcuteTab1(param, userName));
        }
    }
}