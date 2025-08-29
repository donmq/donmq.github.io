using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_2_MonthlyExchangeRateSetting
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(MonthlyExchangeRateSetting_Param param, List<string> roleList);
        Task<OperationResult> IsExistedData(MonthlyExchangeRateSetting_Main param);
        Task<OperationResult> IsDuplicatedData(MonthlyExchangeRateSetting_Main param);
        Task<PaginationUtility<MonthlyExchangeRateSetting_Main>> GetSearchDetail(PaginationParam paginationParams, MonthlyExchangeRateSetting_Param searchParam);
        Task<OperationResult> PutData(MonthlyExchangeRateSetting_Update input);
        Task<OperationResult> PostData(MonthlyExchangeRateSetting_Update input);
        Task<OperationResult> DeleteData(MonthlyExchangeRateSetting_Main data);
        
    }
}