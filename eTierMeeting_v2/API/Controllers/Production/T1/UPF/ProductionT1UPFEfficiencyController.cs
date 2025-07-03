using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionT1UPFEfficiencyController : ControllerBase
    {
        private readonly IProductionT1UPFEfficiencyService _efficiencyUPFService;

        public ProductionT1UPFEfficiencyController(IProductionT1UPFEfficiencyService efficiencyUPFService)
        {
            _efficiencyUPFService = efficiencyUPFService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string deptId)
        {
            return Ok(await _efficiencyUPFService.GetData(deptId));
        }
    }
}