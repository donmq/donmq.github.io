using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.STF
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class ProductionT1STFEfficiencyController : ControllerBase
    {
        private readonly IProductionT1STFEfficiencyService _efficiencySTFService;

        public ProductionT1STFEfficiencyController(IProductionT1STFEfficiencyService efficiencySTFService)
        {
            _efficiencySTFService = efficiencySTFService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string deptId)
        {
            return Ok(await _efficiencySTFService.GetData(deptId));
        }
    }
}