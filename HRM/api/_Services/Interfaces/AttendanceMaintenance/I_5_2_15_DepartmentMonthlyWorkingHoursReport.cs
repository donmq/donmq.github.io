using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_15_DepartmentMonthlyWorkingHoursReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string account);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<OperationResult> GetData(DepartmentMonthlyWorkingHoursReportParam param, string userName); 
        Task<OperationResult> Excel(DepartmentMonthlyWorkingHoursReportParam param, string userName);     
    }
}   