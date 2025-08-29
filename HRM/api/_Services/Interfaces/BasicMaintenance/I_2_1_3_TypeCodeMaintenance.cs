

using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_3_TypeCodeMaintenance
    {
        Task<PaginationUtility<HRMS_Basic_Code_Type_LanguageInfoDto>> Getdata(PaginationParam pagination, HRMS_Basic_Code_TypeParam param);
        Task<OperationResult> AddNew(Language_Dto param, string user);
        Task<OperationResult> AddTypeCode(HRMS_Basic_Code_TypeDto param, string user);
        Task<OperationResult> Edit(HRMS_Basic_Code_TypeDto param);
        Task<OperationResult> EditLanguageCode(Language_Dto param, string user);
        Task<OperationResult> Delete(string Type_Seq);
        Task<List<KeyValuePair<string, string>>> GetLanguage();
        Task<Language_Dto> GetDetail(string type_Seq);
    }
}