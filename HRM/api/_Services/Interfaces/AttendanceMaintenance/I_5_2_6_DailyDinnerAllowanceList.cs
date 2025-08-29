using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_6_DailyDinnerAllowanceList
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(DailyDinnerAllowanceList_Param param, List<string> roleList);
        Task<OperationResult> Search(DailyDinnerAllowanceList_Param param);
        Task<OperationResult> Excel(DailyDinnerAllowanceList_Param param, string userName);
    }
}