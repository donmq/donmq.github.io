using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_27_FinSalaryAttributionCategoryMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(FinSalaryAttributionCategoryMaintenance_Param param, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetDepartmentList(FinSalaryAttributionCategoryMaintenance_Param param);
        Task<List<KeyValuePair<string, string>>> GetKindCodeList(FinSalaryAttributionCategoryMaintenance_Param param);
        Task<OperationResult> GetSearch(PaginationParam paginationParams, FinSalaryAttributionCategoryMaintenance_Param searchParam);
        Task<bool> IsExistedData(FinSalaryAttributionCategoryMaintenance_Param param);
        Task<OperationResult> PutData(FinSalaryAttributionCategoryMaintenance_Data input);
        Task<OperationResult> PostData(FinSalaryAttributionCategoryMaintenance_Update input);
        Task<OperationResult> DeleteData(FinSalaryAttributionCategoryMaintenance_Data data);
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string username);
        Task<OperationResult> DownloadExcel(FinSalaryAttributionCategoryMaintenance_Param param, string userName);
        Task<OperationResult> DownloadExcelTemplate();

    }
}