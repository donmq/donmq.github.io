using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_7_DailyUnregisteredOvertimeList
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(DailyUnregisteredOvertimeList_Param param, List<string> roleList);
        Task<OperationResult> Search(DailyUnregisteredOvertimeList_Param param);
        Task<OperationResult> Excel(DailyUnregisteredOvertimeList_Param param, string userName);
    }
}