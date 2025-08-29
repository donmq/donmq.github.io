using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListLevel(string language);
        Task<int> Search(ResignationAnnualLeaveWorktypeAnalysisReportParam param);
        Task<OperationResult> DownloadExcel(ResignationAnnualLeaveWorktypeAnalysisReportParam param, string userName);
    }
}