using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1UPFModelPreparationController : ControllerBase
    {
        private readonly IProductionT1UPFModelPreparationService _service;

        public ProductionT1UPFModelPreparationController(IProductionT1UPFModelPreparationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetListVideo ([FromQuery]string deptId)
        {
            return Ok(await _service.GetModelPreparation(deptId));
        }

    }
}