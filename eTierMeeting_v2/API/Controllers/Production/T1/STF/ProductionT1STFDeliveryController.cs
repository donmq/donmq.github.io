using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API.Controllers.Production.T1.STF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1STFDeliveryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IProductionT1STFDeliveryService _deliverySTFService;
        public ProductionT1STFDeliveryController(IConfiguration config, IMapper mapper, IProductionT1STFDeliveryService deliverySTFService)
        {
            _config = config;
            _mapper = mapper;
            _deliverySTFService = deliverySTFService;
        }
        [HttpGet("getAllDelivery")]
        public async Task<IActionResult> GetAllDelivery(string deptId)
        {
            var data = await _deliverySTFService.GetData(deptId);
            return Ok(data);
        }

    }
}