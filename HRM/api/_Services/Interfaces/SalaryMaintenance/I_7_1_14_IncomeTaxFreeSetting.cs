using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_14_IncomeTaxFreeSetting
    {
        Task<PaginationUtility<IncomeTaxFreeSetting_MainData>> GetDataPagination(PaginationParam pagination, IncomeTaxFreeSetting_MainParam param);
        Task<List<IncomeTaxFreeSetting_SubData>> GetDetail(IncomeTaxFreeSetting_SubParam param);
        Task<OperationResult> IsDuplicatedData(IncomeTaxFreeSetting_SubParam param, string userName);

        Task<OperationResult> Create(IncomeTaxFreeSetting_Form data);
        Task<OperationResult> Update(IncomeTaxFreeSetting_Form data);
        Task<OperationResult> Delete(IncomeTaxFreeSetting_MainData data);

        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language);
        Task<List<KeyValuePair<string, string>>> GetListType(string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
    }
}