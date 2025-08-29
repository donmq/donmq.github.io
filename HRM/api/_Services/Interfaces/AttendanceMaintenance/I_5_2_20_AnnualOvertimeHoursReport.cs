using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_20_AnnualOvertimeHoursReport
    {
        Task<int> GetTotalRows(AnnualOvertimeHoursReportParam param);
        Task<OperationResult> Download(AnnualOvertimeHoursReportParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
    }
}