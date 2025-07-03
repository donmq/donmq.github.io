using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageEnableDisableController : ControllerBase
    {
        private readonly IPageEnableDisableService _pageEnableDisableService;

        public PageEnableDisableController(IPageEnableDisableService pageEnableDisableService)
        {
            _pageEnableDisableService = pageEnableDisableService;
        }

        [HttpGet("Centers")]
        public async Task<IActionResult> GetCenters()
        {
            var result = await _pageEnableDisableService.GetCenters();
            return Ok(result);
        }

        [HttpGet("Tiers")]
        public async Task<IActionResult> GetTiers([FromQuery] string center_Level)
        {
            var result = await _pageEnableDisableService.GetTiers(center_Level);
            return Ok(result);
        }

        [HttpGet("Sections")]
        public async Task<IActionResult> GetSections([FromQuery] string center_Level, [FromQuery] string tier_Level)
        {
            var result = await _pageEnableDisableService.GetSections(center_Level, tier_Level);
            return Ok(result);
        }

        [HttpGet("Pages")]
        public async Task<IActionResult> GetPages([FromQuery] PageEnableDisableParam param)
        {
            var result = await _pageEnableDisableService.GetPages(param);
            return Ok(result);
        }

        [HttpPut("Pages")]
        public async Task<IActionResult> UpdatePages([FromBody] List<eTM_Page_SettingsDTO> pagesDto)
        {
            var result = await _pageEnableDisableService.UpdatePages(pagesDto);
            return Ok(result);
        }
    }
}