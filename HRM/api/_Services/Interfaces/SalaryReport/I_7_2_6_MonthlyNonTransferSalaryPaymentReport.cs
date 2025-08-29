using API.DTOs.SalaryReport;
namespace API._Services.Interfaces.SalaryReport
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_2_6_MonthlyNonTransferSalaryPaymentReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<int> SearchData(D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam param);
        Task<OperationResult> DownloadFilePdf(D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam param, string userName);
    }
}