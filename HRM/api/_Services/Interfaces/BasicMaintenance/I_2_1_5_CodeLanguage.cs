using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_5_CodeLanguage
    {
        Task<PaginationUtility<Code_LanguageDto>> Search(PaginationParam paginationParams, Code_LanguageParam param, string language);
        Task<Code_LanguageDetail> GetDetail(Code_LanguageParam param);
        Task<List<KeyValuePair<string, string>>> GetLanguage();
        Task<OperationResult> Edit(Code_LanguageDetail model);
        Task<OperationResult> Add(Code_LanguageDetail model);
        Task<OperationResult> Delete(Code_LanguageParam Code);
        Task<List<KeyValuePair<string, string>>> GetTypeSeq();  
        Task<List<KeyValuePair<string, string>>> GetCode(string type_Seq);
        Task<List<string>> GetCodeName(string type_Seq, string code, string language);
        Task<OperationResult> ExportExcel(Code_LanguageParam param, string language);
    }
}