using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.BasicMaintenance
{
    public class C_2_1_7_FactoryComparison : APIController
    {
        private readonly I_2_1_7_FactoryComparison _service;
        public C_2_1_7_FactoryComparison(I_2_1_7_FactoryComparison service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] string kind)
        {
            var result = await _service.GetData(pagination, kind);
            return Ok(result);
        }

        [HttpGet("GetDivisions")]
        public async Task<IActionResult> GetDivisions()
        {
            var result = await _service.GetDivisions();
            return Ok(result);
        }

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories()
        {
            var result = await _service.GetFactories();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<HRMS_Basic_Factory_ComparisonDto> model)
        {

            var result = await _service.Create(model, userName);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] HRMS_Basic_Factory_ComparisonDto model)
        {
            var result = await _service.Delete(model);
            return Ok(result);
        }
    }
}