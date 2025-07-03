
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Production.T1.C2B
{
    [Route("api/[controller]")]
    [ApiController]
    public class T2MeetingTimeSettingController : ControllerBase
    {
        private readonly IT2MeetingTimeSettingService _service;

        public T2MeetingTimeSettingController(IT2MeetingTimeSettingService service)
        {
            _service = service;
        }

        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllData([FromQuery]Helpers.Params.PaginationParam pagination,[FromQuery] T2MeetingTimeSettingParam param)
        { 
            var result = await _service.GetAllData(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListBuildingOrGroup")]
        public async Task<IActionResult> GetListBuildingOrGroup()
        { 
            var result = await _service.GetListBuildingOrGroup();
            return Ok(result);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO)
        { 
            eTM_T2_Meeting_SeetingDTO.Update_By = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            eTM_T2_Meeting_SeetingDTO.Update_At = DateTime.Now;
            var result = await _service.Add(eTM_T2_Meeting_SeetingDTO);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery]eTM_T2_Meeting_SeetingDTO eTM_T2_Meeting_SeetingDTO)
        { 
            var result = await _service.Delete(eTM_T2_Meeting_SeetingDTO);
            return Ok(result);
        }


    }
}