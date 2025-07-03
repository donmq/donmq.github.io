using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ProductionT1STFSafetyController : ControllerBase
    {
        private readonly IProductionT1STFSafetyService _productionT1STFSafetyService;

        public ProductionT1STFSafetyController(IProductionT1STFSafetyService productionT1STFSafetyService)
        {
            _productionT1STFSafetyService = productionT1STFSafetyService;
        }

        [HttpGet("TodayData")]
        public async Task<IActionResult> GetTodayData([FromQuery] string deptId)
        {
            var result = await _productionT1STFSafetyService.GetTodayData(deptId);
            return Ok(result);
        }
    }
}