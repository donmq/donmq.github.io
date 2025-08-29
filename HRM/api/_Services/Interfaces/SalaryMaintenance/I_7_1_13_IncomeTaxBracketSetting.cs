using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_13_IncomeTaxBracketSetting
    {
        Task<PaginationUtility<IncomeTaxBracketSettingMain>> GetDataPagination(PaginationParam pagination, IncomeTaxBracketSettingParam param);
        Task<IncomeTaxBracketSettingDto> GetDetail(IncomeTaxBracketSettingDto dto);
        Task<OperationResult> Create(IncomeTaxBracketSettingDto dto, string userName);
        Task<OperationResult> Update(IncomeTaxBracketSettingDto dto, string userName);
        Task<OperationResult> Delete(IncomeTaxBracketSettingMain dto);
        Task<List<KeyValuePair<string, string>>> GetListNationality(string Language);
        Task<List<KeyValuePair<string, IncomeTaxBracketSettingSub>>> GetListTaxCode(string Language);
        Task<OperationResult> IsDuplicatedData(string Nation,string Tax_Code, int Tax_Level, string Effective_Month);
    }
}