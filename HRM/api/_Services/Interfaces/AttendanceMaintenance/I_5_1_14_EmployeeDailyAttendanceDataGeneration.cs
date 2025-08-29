using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_14_EmployeeDailyAttendanceDataGeneration
    {
        Task<List<KeyValuePair<string, string>>> GetFactories(string language, string userName);
        Task<OperationResult> GetHolidayDates(string factory, string offWork, string workDay);
        Task<OperationResult> GetNationalHolidays(string factory, string offWork, string workDay);
        Task<OperationResult> CheckClockInDateInCurrentDate(string factory, string card_Date);
        Task<OperationResult> ExcuteQuery(HRMS_Att_Swipe_Card_Excute_Param param);
    }
}