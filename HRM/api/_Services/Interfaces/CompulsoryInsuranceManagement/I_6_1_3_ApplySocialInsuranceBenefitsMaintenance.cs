
using API.DTOs.CompulsoryInsuranceManagement;

namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_1_3_ApplySocialInsuranceBenefitsMaintenance
    {
        Task<PaginationUtility<ApplySocialInsuranceBenefitsMaintenanceDto>> GetDataPagination(PaginationParam pagination, ApplySocialInsuranceBenefitsMaintenanceParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListBenefitsKind(string language);
        Task<OperationResult> Create(ApplySocialInsuranceBenefitsMaintenanceDto data);
        Task<OperationResult> Update(ApplySocialInsuranceBenefitsMaintenanceDto data);
        Task<OperationResult> Delete(ApplySocialInsuranceBenefitsMaintenanceDto data);
        Task<OperationResult> Formula(ApplySocialInsuranceBenefitsMaintenanceDto data);
        Task<OperationResult> GetAdditionData(ApplySocialInsuranceBenefitsMaintenanceDto data);
        Task<List<string>> GetListTypeHeadEmployeeID(string factory);
        Task<OperationResult> DownloadExcel(ApplySocialInsuranceBenefitsMaintenanceParam param, string userName);
        Task<OperationResult> CheckDate(ApplySocialInsuranceBenefitsMaintenanceDto data);
    }
}