using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.STF
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class ProductionT1STFKaizenController : ControllerBase
    {
        private readonly IProductionT1STFKaizenService _productionT1STFKaizenService;

        public ProductionT1STFKaizenController(IProductionT1STFKaizenService productionT1STFKaizenService)
        {
            _productionT1STFKaizenService = productionT1STFKaizenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListVideo(string deptId, string date)
        {
            return Ok(await _productionT1STFKaizenService.GetListVideo(deptId, date));
        }
    }
}