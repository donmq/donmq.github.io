using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.Helpers.Params.Production.T2.CTB;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T2.CTB
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionT2CTBEfficiencyController : ControllerBase
    {
        readonly IProductionT2CTBEfficiencyService _service;
        public ProductionT2CTBEfficiencyController(IProductionT2CTBEfficiencyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] Params param)
        {
            return Ok(await _service.GetData(param.deptId, param.check));
        }
    }
}