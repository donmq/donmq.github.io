using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_10_SalaryItemToAccountingCodeMappingMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetFactory(string userName, string language);
        Task<PaginationUtility<D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto>> GetDataPagination(PaginationParam pagination, D_7_10_SalaryItemToAccountingCodeMappingMaintenanceParam param);
        Task<OperationResult> Create(D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto model, string userName);
        Task<OperationResult> Edit(D_7_10_SalaryItemToAccountingCodeMappingMaintenanceDto models);
        Task<OperationResult> Delete(string factory, string Salary_Item, string DC_Code);
        Task<OperationResult> CheckDupplicate(string factory, string Salary_Item, string DC_Code);
    }
}