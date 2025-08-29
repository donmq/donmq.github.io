using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
    public class C_6_1_2_ContributionRateSetting : APIController
    {
        private readonly I_6_1_2_ContributionRateSetting _service;
        public C_6_1_2_ContributionRateSetting(I_6_1_2_ContributionRateSetting service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] ContributionRateSettingParam param)
        {
            var data = await _service.GetDataPagination(pagination, param);
            return Ok(data);
        }
        [HttpGet("GetDetail")]
        public async Task<ActionResult> GetDetail([FromQuery] ContributionRateSettingSubParam param)
        {
            var data = await _service.GetDetail(param);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ContributionRateSettingForm data)
        {
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] ContributionRateSettingForm data)
        {
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] ContributionRateSettingDto data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(roleList, language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup(string factory, string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }

        [HttpGet("GetListInsuranceType")]
        public async Task<IActionResult> GetListInsuranceType(string language)
        {
            return Ok(await _service.GetListInsuranceType(language));
        }

        [HttpGet("CheckSearch")]
        public async Task<IActionResult> CheckSearch([FromQuery] ContributionRateSettingParam param)
        {
            return Ok(await _service.CheckSearch(param));
        }

        [HttpGet("CheckEffectiveMonth")]
        public async Task<IActionResult> CheckEffectiveMonth([FromQuery] ContributionRateSettingSubParam param)
        {
            return Ok(await _service.CheckEffectiveMonth(param));
        }
    }
}