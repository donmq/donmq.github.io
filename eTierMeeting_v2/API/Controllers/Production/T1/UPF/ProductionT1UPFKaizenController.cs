using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1UPFKaizenController : ControllerBase
    {
        private readonly IProductionT1UPFKaizenService _service;

        public ProductionT1UPFKaizenController(IProductionT1UPFKaizenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetListVideo([FromQuery] string deptId)
        {
            return Ok(await _service.GetListVideo(deptId));
        }
    }
}