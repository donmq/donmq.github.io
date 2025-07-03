using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class QualityController : ControllerBase
    {
        private readonly IProductionT1QualityService _qualityService;
        public QualityController(IProductionT1QualityService qualityService)
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
        [HttpGet("getBADefectTop3Chart")]
        public async Task<IActionResult> GetBADefectTop3Chart([FromQuery] string deptId)
        {
            var data = await _qualityService.GetBADefectTop3Chart(deptId);
            return Ok(data);
        }

    }
}