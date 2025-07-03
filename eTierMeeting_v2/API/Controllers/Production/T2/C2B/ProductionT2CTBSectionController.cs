using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T2.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionT2CTBSectionController : Controller
    {
        private readonly IProductionT2CTBSectionService _productionT2CTBSection;

        public ProductionT2CTBSectionController(IProductionT2CTBSectionService productionT2CTBSection)
        {
            _productionT2CTBSection = productionT2CTBSection;
        }

        [HttpGet("GetListProductionT2CTB")]
        public async Task<IActionResult> GetListProductionT2CTB() => Ok(await _productionT2CTBSection.getFactoryIndex());

    }
}