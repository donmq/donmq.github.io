using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_8_DailyNoNightShiftHoursList
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(DailyNoNightShiftHoursList_Param param, List<string> roleList);
        Task<OperationResult> Search(DailyNoNightShiftHoursList_Param param);
        Task<OperationResult> Excel(DailyNoNightShiftHoursList_Param param, string userName);
    }
}