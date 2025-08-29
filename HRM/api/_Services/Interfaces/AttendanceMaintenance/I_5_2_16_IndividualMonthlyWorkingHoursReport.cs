using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_16_IndividualMonthlyWorkingHoursReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string account);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<OperationResult> GetData(IndividualMonthlyWorkingHoursReportParam param, string userName);
        Task<OperationResult> Excel(IndividualMonthlyWorkingHoursReportParam param, string userName);
    }
}