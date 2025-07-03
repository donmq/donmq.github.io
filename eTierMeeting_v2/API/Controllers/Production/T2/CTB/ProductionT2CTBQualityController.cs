using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Services.Interfaces.Production.T2.C2B;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProductionT2CTBQualityController : ControllerBase
    {
        private readonly IProductionT2CTBQualityService _qualityService;
        public ProductionT2CTBQualityController(IProductionT2CTBQualityService qualityService)
        {
            _qualityService = qualityService;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] string tuCode, bool switchDate)
        {
            var data = await _qualityService.GetData(tuCode, switchDate);
            return Ok(data);
        }

        [HttpGet("GetDefectTop3Photos")]
        public async Task<IActionResult> GetDefectTop3Photos([FromQuery] string tuCode, bool switchDate)
        {
            var data = await _qualityService.GetDefectTop3Photos(tuCode, switchDate);
            return Ok(data);
        }
        [HttpGet("GetBADefectTop3Chart")]
        public async Task<IActionResult> GetBADefectTop3Chart([FromQuery] string tuCode, bool switchDate)
        {
            var data = await _qualityService.GetBADefectTop3Chart(tuCode, switchDate);
            return Ok(data);
        }

    }
}