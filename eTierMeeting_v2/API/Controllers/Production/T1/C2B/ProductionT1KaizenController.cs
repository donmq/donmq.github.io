using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class ProductionT1KaizenController : ControllerBase
    {
        private readonly IProductionT1KaizenService _productionT1KaizenService;

        public ProductionT1KaizenController(IProductionT1KaizenService productionT1KaizenService)
        {
            _productionT1KaizenService = productionT1KaizenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListVideo(string deptId)
        {
            return Ok(await _productionT1KaizenService.GetListVideo(deptId));
        }
    }
}