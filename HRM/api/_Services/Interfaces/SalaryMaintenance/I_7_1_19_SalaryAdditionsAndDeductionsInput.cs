using API.DTOs.SalaryMaintenance;
namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_19_SalaryAdditionsAndDeductionsInput
    {
        Task<PaginationUtility<SalaryAdditionsAndDeductionsInputDto>> GetDataPagination(PaginationParam pagination, SalaryAdditionsAndDeductionsInput_Param param);
        Task<OperationResult> Create(SalaryAdditionsAndDeductionsInputDto dto);
        Task<OperationResult> Update(SalaryAdditionsAndDeductionsInputDto dto);
        Task<OperationResult> Delete(SalaryAdditionsAndDeductionsInputDto dto);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<KeyValuePair<string, string>>> GetListAddDedType(string language);
        Task<List<KeyValuePair<string, string>>> GetListCurrency(string language);
        Task<List<KeyValuePair<string, string>>> GetListAddDedItem(string language);
        Task<OperationResult> DownloadFileExcel(SalaryAdditionsAndDeductionsInput_Param param, string userName);
        Task<OperationResult> DownloadFileTemplate();
        Task<OperationResult> UploadFileExcel(SalaryAdditionsAndDeductionsInput_Upload param, List<string> roleList, string userName);
    }
}