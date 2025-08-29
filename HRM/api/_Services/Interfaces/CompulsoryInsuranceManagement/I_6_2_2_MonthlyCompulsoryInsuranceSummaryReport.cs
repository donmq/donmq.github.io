using API.DTOs.CompulsoryInsuranceManagement;

namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_2_2_MonthlyCompulsoryInsuranceSummaryReport
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<OperationResult> DownLoadExcel(MonthlyCompulsoryInsuranceSummaryReport_Param param, string userName);
        Task<int> GetCountRecords(MonthlyCompulsoryInsuranceSummaryReport_Param param);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
    }
}