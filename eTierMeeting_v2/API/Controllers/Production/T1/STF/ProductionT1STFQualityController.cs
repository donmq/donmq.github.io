using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.STF
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProductionT1STFQualityController : ControllerBase
    {
        private readonly IProductionT1STFQualityService _qualityService;
        public ProductionT1STFQualityController(IProductionT1STFQualityService qualityService)
        {
            _qualityService = qualityService;
        }

        [HttpGet("getData")]
        public async Task<IActionResult> GetData([FromQuery] string deptId)
        {
            var data = await _qualityService.GetData(deptId);
            return Ok(data);
        }

        [HttpGet("getDefectTop3")]
        public async Task<IActionResult> GetDefectTop3([FromQuery] string deptId)
        {
            var data = await _qualityService.GetDefectTop3(deptId);
            return Ok(data);
        }

    }
}