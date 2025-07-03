using System.Security.Claims;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Params.Production.T2.CTB;
using eTierV2_API.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T2.CTB
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageItemSettingController : Controller
    {
        private readonly IPageItemSettingService _pageItemSettingService;

        public PageItemSettingController(IPageItemSettingService pageItemSettingService)
        {
            _pageItemSettingService = pageItemSettingService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] PageItemSettingParam param, [FromQuery] PaginationParam pagination)
        {
            var result = await _pageItemSettingService.GetAll(param, pagination);
            return Ok(result);
        }

        [HttpGet("CenterLevels")]
        public async Task<IActionResult> GetCenterLevels()
        {
            var result = await _pageItemSettingService.GetCenterLevels();
            return Ok(result);
        }

        [HttpGet("TierLevels")]
        public async Task<IActionResult> GetTierLevels([FromQuery] string center_Level)
        {
            var result = await _pageItemSettingService.GetTierLevels(center_Level);
            return Ok(result);
        }

        [HttpGet("Sections")]
        public async Task<IActionResult> Sections([FromQuery] string center_Level, [FromQuery] string tier_Level)
        {
            var result = await _pageItemSettingService.GetSections(center_Level, tier_Level);
            return Ok(result);
        }

        [HttpGet("Pages")]
        public async Task<IActionResult> GetPages()
        {
            var result = await _pageItemSettingService.GetPages();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] eTM_Page_Item_SettingsDTO settingDTO)
        {
            var update_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _pageItemSettingService.Add(settingDTO, update_By);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] eTM_Page_Item_SettingsDTO settingDTO)
        {
            var update_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _pageItemSettingService.Update(settingDTO, update_By);
            return Ok(result);
        }
    }
}