using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class EfficiencyController : ControllerBase
    {
        private readonly IEfficiencyService _efficiencyService;

        public EfficiencyController(IEfficiencyService efficiencyService)
        {
            _efficiencyService = efficiencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string deptId)
        {
            return Ok(await _efficiencyService.GetData(deptId));
        }

        [HttpGet("GetDataChart")]
        public async Task<IActionResult> GetDataChart(string deptId)
        {
            return Ok(await _efficiencyService.GetDataChart(deptId));
        }
    }
}