using System.Security.Claims;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.HolidayManage;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet("GetHoliday")]
        public async Task<IActionResult> GetHoliday()
        {
            var data = await _holidayService.GetHolidayData();
            return Ok(data);
        }

        [HttpPost("AddHoliday")]
        public async Task<IActionResult> AddHoliday([FromBody] HolidayDto Holidays)
        {
            Holidays.UserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Holidays.CreateTime = DateTime.Now;
            var data = await _holidayService.AddHoliday(Holidays);
            return Ok(data);
        }

        [HttpDelete("RemoveHoliday")]
        public async Task<IActionResult> RemoveHoliday([FromQuery] int IDHoliday)
        {
            var data = await _holidayService.RemoveHoliday(IDHoliday);
            return Ok(data);
        }
    }
}