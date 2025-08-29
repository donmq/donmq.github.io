using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_21_EmployeeOvertimeExceedingHoursReport
    {
        Task<int> GetTotalRows(EmployeeOvertimeExceedingHoursReportParam param);
        Task<OperationResult> Download(EmployeeOvertimeExceedingHoursReportParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
    }
}