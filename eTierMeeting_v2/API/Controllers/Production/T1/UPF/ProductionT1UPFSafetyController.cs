using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1UPFSafetyController : ControllerBase
    {
        private readonly IProductionT1UPFSafetyService _productionT1UPFSafetyService;
        public ProductionT1UPFSafetyController(IProductionT1UPFSafetyService productionT1UPFSafetyService)
        {
            _productionT1UPFSafetyService = productionT1UPFSafetyService;
        }

        [HttpGet("GetTodayData")]
        public async Task<IActionResult> GetTodayData([FromQuery] string deptId)
        {
            var result = await _productionT1UPFSafetyService.GetTodayData(deptId);
            return Ok(result);
        }

    }
}