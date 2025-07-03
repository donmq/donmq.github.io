using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces;
using eTierV2_API.DTO;
using eTierV2_API.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;

        public CommonController(
            ICommonService commonService,
            IConfiguration configuration)
        {
            _commonService = commonService;
            _configuration = configuration;
        }

        [HttpGet("getLineID")]
        public async Task<ActionResult> GetLineID([FromQuery] string deptID)
        {
            var data = await _commonService.GetLineID(deptID);
            return Ok(data);
        }

        [HttpGet("GetRouteT5")]
        public async Task<ActionResult> GetRouteT5()
        {
            var data = await _commonService.GetRouteT5();
            return Ok(data);
        }

        [HttpGet("ServerInfo")]
        public async Task<IActionResult> GetServerInfo()
        {
            var factory = _configuration.GetSection("Appsettings:Factory").Value;
            var area = _configuration.GetSection("Appsettings:Area").Value;
            return Ok(await Task.FromResult(new { factory, area }));
        }

        [HttpGet("AddVideoLog")]
        public async Task<ActionResult<string>> AddVideoLog([FromQuery] eTM_Video_Play_LogDTO videoLogDTO)
        {
            var data = await _commonService.AddVideoLog(videoLogDTO);
            return Ok(data);
        }

        [HttpPost("AddMeetingLogPage")]
        public async Task<ActionResult<string>> AddMeetingLogPage([FromBody] eTM_Meeting_Log_PageDTO logPageDto)
        {
            var data = await _commonService.AddMeetingLogPage(logPageDto);
            return Ok(data);
        }

        [HttpPut("UpdateMeetingLogPage")]
        public async Task<ActionResult<string>> UpdateMeetingLogPage([FromBody] eTM_Meeting_Log_PageParamDTO param)
        {
            OperationResult data = await _commonService.UpdateMeetingLogPage(param.Record_ID, param.ClickLinkKaizenSystem);
            return Ok(data);
        }
    }
}
