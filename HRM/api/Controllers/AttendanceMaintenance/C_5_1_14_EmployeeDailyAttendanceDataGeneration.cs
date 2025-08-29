using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_14_EmployeeDailyAttendanceDataGeneration : APIController
    {
        private readonly I_5_1_14_EmployeeDailyAttendanceDataGeneration _services;
        public C_5_1_14_EmployeeDailyAttendanceDataGeneration(I_5_1_14_EmployeeDailyAttendanceDataGeneration services)
        {
            _services = services;
        }


        [HttpGet("GetFactories")]
        public async Task<IActionResult> GetFactories(string language)
        {
            var result = await _services.GetFactories(language,userName);
            return Ok(result);
        }

        [HttpGet("CheckClockInDateInCurrentDate")]
        public async Task<IActionResult> CheckClockInDateInCurrentDate(string factory, string card_Date)
        {
            var result = await _services.CheckClockInDateInCurrentDate(factory, card_Date);
            return Ok(result);
        }

        [HttpGet("GetHolidays")]
        public async Task<IActionResult> GetHolidays(string factory, string offWork, string workDay)
        {
            var result = await _services.GetHolidayDates(factory, offWork, workDay);
            return Ok(result);
        }

        [HttpGet("GetNationalHolidays")]
        public async Task<IActionResult> GetNationalHolidays(string factory, string offWork, string workDay)
        {
            var result = await _services.GetNationalHolidays(factory, offWork, workDay);
            return Ok(result);
        }

        // [AllowAnonymous]
        [HttpPost("Excute")]
        public async Task<IActionResult> Excute([FromBody] HRMS_Att_Swipe_Card_Excute_Param param)
        {
            // Fake Data 
            // param.Factory = "SHC";
            // param.CurrentUser = "adminshc";
            // param.ClockOffDay = "2024-06-22";
            // param.WorkOnDay = "2024-06-17";
            // param.Holiday = "2024-06-19";
            // param.NationalHoliday = "2024-06-21";

            param.CurrentUser = userName;
            var result = await _services.ExcuteQuery(param);
            return Ok(result);
        }
    }
}