using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_12_AdditionDeductionItemAndAmountSettings
    {
        Task<PaginationUtility<AdditionDeductionItemAndAmountSettingsDto>> GetDataPagination(PaginationParam pagination, AdditionDeductionItemAndAmountSettingsParam param);
        Task<AdditionDeductionItemAndAmountSettings_Form> GetDetail(AdditionDeductionItemAndAmountSettings_SubParam param);
        Task<OperationResult> CheckData(AdditionDeductionItemAndAmountSettings_SubParam param, string userName);

        Task<OperationResult> Create(AdditionDeductionItemAndAmountSettings_Form dto);
        Task<OperationResult> Update(AdditionDeductionItemAndAmountSettings_Form dto);
        Task<OperationResult> Delete(AdditionDeductionItemAndAmountSettingsDto dto);
        Task<OperationResult> DownloadFileExcel(AdditionDeductionItemAndAmountSettingsParam param, string userName);

        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroupByFactory(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<List<KeyValuePair<string, string>>> GetListAdditionsAndDeductionsType(string language);
        Task<List<KeyValuePair<string, string>>> GetListAdditionsAndDeductionsItem(string language);
    }
}