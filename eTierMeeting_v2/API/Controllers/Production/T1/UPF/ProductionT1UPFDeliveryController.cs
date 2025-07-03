using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.UPF
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT1UPFDeliveryController : ControllerBase
    {
        private readonly IProductionT1UPFDeliveryService _service;

        public ProductionT1UPFDeliveryController(IProductionT1UPFDeliveryService service)
        {
            _service = service;
        }
        [HttpGet("getAllDelivery")]
        public async Task<IActionResult> GetAllDelivery([FromQuery]string deptId)
        {
            var data = await _service.GetData(deptId);
            return Ok(data);
        }
    }
}