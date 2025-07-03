using System.Threading.Tasks;
using AutoMapper;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IDeliveryService _deliveryService;
        public DeliveryController(IConfiguration config, IMapper mapper, IDeliveryService deliveryService)
        {
            _config = config;
            _mapper = mapper;
            _deliveryService = deliveryService;
        }
        [HttpGet("getAllDelivery")]
        public async Task<IActionResult> GetAllDelivery(string deptId)
        {
            var data = await _deliveryService.GetAllDelivery(deptId);
            return Ok(data);
        }

    }
}