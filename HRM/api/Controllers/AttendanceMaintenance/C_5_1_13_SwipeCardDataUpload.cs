using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{

    public class C_5_1_13_SwipeCardDataUpload : APIController
    {
        private readonly I_5_1_13_SwipeCardDataUpload _services;
        public C_5_1_13_SwipeCardDataUpload(I_5_1_13_SwipeCardDataUpload services)
        {
            _services = services;
        }

        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetFactories(language, roleList);
            return Ok(result);
        }

        [HttpPost("Excute")]
        public async Task<IActionResult> Excute([FromForm] HRMS_Att_Swipe_Card_Upload request)
        {
            request.CurrentUser = userName;
            var result = await _services.UploadExcuteSwipeData(request);
            return Ok(result);
        }
    }
}