using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.STF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1STFModelPreparationController : ControllerBase
    {
        private readonly IProductionT1STFModelPreparationService _productionT1STFModelPreparationService;

        public ProductionT1STFModelPreparationController(IProductionT1STFModelPreparationService productionT1STFModelPreparationService)
        {
            _productionT1STFModelPreparationService = productionT1STFModelPreparationService;
        }

        [HttpGet("GetModelPreparation")]
        public async Task<IActionResult> GetModelPreparation([FromQuery] string deptId)
        {
            var result = await _productionT1STFModelPreparationService.GetModelPreparation(deptId);
            return Ok(result);
        }
    }
}