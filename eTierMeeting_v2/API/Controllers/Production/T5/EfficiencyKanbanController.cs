using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T5;
using eTierV2_API.DTO.Production.T5.EfficiencyKanban;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Controllers.Production.T5
{
    [Route("api/[controller]")]
    [ApiController]
    public class EfficiencyKanbanController : ControllerBase
    {
        private readonly IEfficiencyKanbanService _serviceEfficiencyKanban;
        public EfficiencyKanbanController(IEfficiencyKanbanService serviceEfficiencyKanban)
        {
            _serviceEfficiencyKanban = serviceEfficiencyKanban;
        }

        [HttpPost("getData")]
        public async Task<IActionResult> GetData(EffiencyKanbanParam param) {
            var data = await _serviceEfficiencyKanban.GetDataChart(param);
            return Ok(data);
        }
        [HttpGet("getListFactory")]
        public async Task<IActionResult> GetListFactory() {
            var data = await _serviceEfficiencyKanban.GetListFactory();
            return Ok(data);
        }
    }
}