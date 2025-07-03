using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1ModelPreparationController : ControllerBase
    {
        private readonly IProductionT1ModelPreparationService _productionT1ModelPreparationService;

        public ProductionT1ModelPreparationController(IProductionT1ModelPreparationService productionT1ModelPreparationService)
        {
            _productionT1ModelPreparationService = productionT1ModelPreparationService;
        }

        [HttpGet("GetModelPreparation")]
        public async Task<IActionResult> GetModelPreparation([FromQuery] string deptId)
        {
            var result = await _productionT1ModelPreparationService.GetModelPreparation(deptId);
            return Ok(result);
        }
    }
}