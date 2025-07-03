using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1UPFQualityController : ControllerBase
    {
        private readonly IProductionT1UPFQualityService _productionT1UPFQualityService;
        public ProductionT1UPFQualityController(IProductionT1UPFQualityService productionT1UPFQualityService)
        {
            _productionT1UPFQualityService = productionT1UPFQualityService;

        }

        [HttpGet("GetData")]
        public  async Task<IActionResult> GetData([FromQuery] string deptId){
            var result = await _productionT1UPFQualityService.GetData(deptId); 
            return Ok(result);
        }

        [HttpGet("GetDefectTop3")]
        public async Task<IActionResult> GetDefectTop3([FromQuery] string deptId){
            var result = await _productionT1UPFQualityService.GetDefectTop3(deptId);
            return Ok(result);
        }
    }
}