using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_5_DailySwipeCardAnomaliesList
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(DailySwipeCardAnomaliesList_Param param, List<string> roleList);
        Task<OperationResult> Search(DailySwipeCardAnomaliesList_Param param);
        Task<OperationResult> Excel(DailySwipeCardAnomaliesList_Param param, string userName);
    }
}