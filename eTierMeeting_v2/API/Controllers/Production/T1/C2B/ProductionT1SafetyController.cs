using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProductionT1SafetyController : ControllerBase
    {
        private readonly IProductionT1SafetyService _productionT1SafetyService;

        public ProductionT1SafetyController(IProductionT1SafetyService productionT1SafetyService)
        {
            _productionT1SafetyService = productionT1SafetyService;
        }

        [HttpGet("TodayData")]
        public async Task<IActionResult> GetTodayData([FromQuery] string deptId)
        {
            var result = await _productionT1SafetyService.GetTodayData(deptId);
            return Ok(result);
        }
    }
}