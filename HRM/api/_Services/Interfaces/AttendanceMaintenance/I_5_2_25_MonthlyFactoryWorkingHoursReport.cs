using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_25_MonthlyFactoryWorkingHoursReport
    {
        Task<int> GetTotalRows(MonthlyFactoryWorkingHoursReportParam param);
        Task<OperationResult> Download(MonthlyFactoryWorkingHoursReportParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
    }
}