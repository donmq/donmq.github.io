using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T2.CTB
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionT2SafetyController : ControllerBase
    {
        private readonly IProductionT2SafetyService _productionT2SafetyService;
        public ProductionT2SafetyController(IProductionT2SafetyService productionT2SafetyService)
        {
            _productionT2SafetyService = productionT2SafetyService;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(string building)
        {
            return Ok(await _productionT2SafetyService.GetData(building));
        }

        [HttpGet("GetDetailScoreUnPass")]
        public async Task<IActionResult> GetDetailScoreUnPass(int hseScoreID)
        {
            return Ok(await _productionT2SafetyService.GetDetailScoreUnPass(hseScoreID));
        }
    }
}