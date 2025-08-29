using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_17_MonthlyWorkingHoursLeaveHoursReport
    {
        Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<OperationResult> ExportExcel(MonthlyWorkingHoursLeaveHoursReportParam param);
        Task<int> GetTotalRows(MonthlyWorkingHoursLeaveHoursReportParam param);
    }
}