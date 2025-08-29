using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_3_WeeklyWorkingHoursReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListLevel(string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<int> GetCountRecords(WeeklyWorkingHoursReportParam param);
        Task<OperationResult> DownloadFileExcel(WeeklyWorkingHoursReportParam param, string userName);
    }
}