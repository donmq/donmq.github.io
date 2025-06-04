
using System.Security.Claims;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.DatepickerManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatepickerController : ControllerBase
    {
        private readonly IDatepickerService _datepickerService;

        public DatepickerController(IDatepickerService  datepickerService)
        {
            _datepickerService = datepickerService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _datepickerService.GetAll();
            return Ok(data);
        }

        [HttpPut("UpdateDatepicker")]
        public async Task<IActionResult> UpdateDatepicker([FromBody] DatepickerDto datepickerDto)
        {
            var UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var data = await _datepickerService.UpdateDatepicker(datepickerDto, Convert.ToInt32(UserID));
            return Ok(data);
        }
    }

}
