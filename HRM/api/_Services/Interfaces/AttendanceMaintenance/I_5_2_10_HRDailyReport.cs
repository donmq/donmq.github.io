using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance;
[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_5_2_10_HRDailyReport
{
    Task<OperationResult> GetTotalRows(HRDailyReportParam param, List<string> roleList, string userName);
    Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
    Task<List<KeyValuePair<string, string>>> GetListLevel(string language);
    Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
    Task<OperationResult> DownloadExcel(HRDailyReportParam param, List<string> roleList, string userName);
}
