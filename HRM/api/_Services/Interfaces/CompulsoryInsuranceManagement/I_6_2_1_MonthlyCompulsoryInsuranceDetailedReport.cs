namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{

    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_2_1_MonthlyCompulsoryInsuranceDetailedReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroupByFactory(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetDepartments(string factory, string language);
        Task<int> GetCountRecords(D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param);
        Task<OperationResult> DownloadFileExcel(D_6_2_1_MonthlyCompulsoryInsuranceDetailedReportParam param, string userName);
    }
}