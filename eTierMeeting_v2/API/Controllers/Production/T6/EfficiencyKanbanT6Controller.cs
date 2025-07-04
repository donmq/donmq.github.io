using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T6;
using eTierV2_API.DTO.Production.T6.EfficiencyKanban;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T6
{
    [Route("api/[controller]")]
    [ApiController]
    public class EfficiencyKanbanT6Controller : ControllerBase
    {
        private readonly IEfficiencyKanbanT6Service _serviceEfficiencyKanban;
        public EfficiencyKanbanT6Controller(IEfficiencyKanbanT6Service serviceEfficiencyKanban)
        {
            _serviceEfficiencyKanban = serviceEfficiencyKanban;
        }

        [HttpPost("getData")]
        public async Task<IActionResult> GetData(EffiencyKanbanParam param)
        {
            var data = await _serviceEfficiencyKanban.GetDataChart(param);
            return Ok(data);
        }
        [HttpGet("getListFactory")]
        public async Task<IActionResult> GetListFactory()
        {
            var data = await _serviceEfficiencyKanban.GetListFactory();
            return Ok(data);
        }
        [HttpGet("getListBrand")]
        public async Task<IActionResult> GetListBrand()
        {
            var data = await _serviceEfficiencyKanban.GetListBrand();
            return Ok(data);
        }
    }
}