using System;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordMeetingDurationController : ControllerBase
    {
        private readonly IRecordMeetingDurationService _recordMeetingDurationService;

        public RecordMeetingDurationController(IRecordMeetingDurationService recordMeetingDurationService)
        {
            _recordMeetingDurationService = recordMeetingDurationService;
        }

        [HttpGet("MeetingLog")]
        public async Task<IActionResult> GetMeetingLog(Guid record_ID)
        {
            var result = await _recordMeetingDurationService.GetMeetingLog(record_ID);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string deptId, string classType, string tierLevel)
        {
            return Ok(await _recordMeetingDurationService.Create(deptId, classType, tierLevel));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(eTM_Meeting_LogDTO mettingLogDto)
        {
            return Ok(await _recordMeetingDurationService.Update(mettingLogDto));
        }
    }
}