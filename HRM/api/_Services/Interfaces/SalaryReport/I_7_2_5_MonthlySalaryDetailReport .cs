using API.DTOs.SalaryReport;

namespace API._Services.Interfaces.SalaryReport
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_2_5_MonthlySalaryDetailReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<int> GetTotal(MonthlySalaryDetailReportParam param);
        Task<OperationResult> DownloadExcel(MonthlySalaryDetailReportParam param, string userName);
    }
}