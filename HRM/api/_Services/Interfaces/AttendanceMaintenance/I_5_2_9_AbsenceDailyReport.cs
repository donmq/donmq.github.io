using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_9_AbsenceDailyReport
    {
        Task<AbsenceDailyReportCount> GetTotalRows(AbsenceDailyReportParam param, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
        Task<OperationResult> DownloadExcel(AbsenceDailyReportParam param, List<string> roleList, string userName);
    }
}