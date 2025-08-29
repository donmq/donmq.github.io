using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_2_WorkingHoursReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<WorkingHoursReportDto>> GetData(WorkingHoursReportParam param);
        Task<int> GetTotal(WorkingHoursReportParam param);
        Task<OperationResult> DownloadExcel(WorkingHoursReportParam param, string userName);
    }
}