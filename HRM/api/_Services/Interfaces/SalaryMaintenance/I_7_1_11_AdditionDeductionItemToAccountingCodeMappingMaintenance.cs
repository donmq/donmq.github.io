using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance
    {
        Task<PaginationUtility<AdditionDeductionItemToAccountingCodeMappingMaintenanceDto>> GetDataPagination(PaginationParam pagination, AdditionDeductionItemToAccountingCodeMappingMaintenanceParam param);
        // Task<AdditionDeductionItemToAccountingCodeMappingMaintenanceDto> GetDetail();
        Task<OperationResult> Create(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto, string userName);
        Task<OperationResult> Update(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto, string userName);
        Task<OperationResult> Delete(AdditionDeductionItemToAccountingCodeMappingMaintenanceDto dto);

        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListAdditionsAndDeductionsItem(string language);
    }
}