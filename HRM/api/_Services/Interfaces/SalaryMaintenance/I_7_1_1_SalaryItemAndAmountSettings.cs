using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_1_SalaryItemAndAmountSettings
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(SalaryItemAndAmountSettings_MainParam param, List<string> roleList);
        Task<OperationResult> IsExistedData(SalaryItemAndAmountSettings_MainData param);
        Task<OperationResult> IsDuplicatedData(SalaryItemAndAmountSettings_SubParam param, string userName);
        Task<PaginationUtility<SalaryItemAndAmountSettings_MainData>> GetSearchDetail(PaginationParam paginationParams, SalaryItemAndAmountSettings_MainParam searchParam);
        Task<OperationResult> PutData(SalaryItemAndAmountSettings_Update input);
        Task<OperationResult> PostData(SalaryItemAndAmountSettings_Update input);
        Task<OperationResult> DeleteData(SalaryItemAndAmountSettings_MainData data);
        
    }
}