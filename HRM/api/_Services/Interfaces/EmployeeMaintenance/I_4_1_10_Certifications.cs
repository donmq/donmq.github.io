using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_10_Certifications
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(Certifications_MainParam param);
        Task<List<Certifications_TypeheadKeyValue>> GetEmployeeList(Certifications_SubParam param);
        Task<PaginationUtility<Certifications_MainData>> GetSearchDetail(PaginationParam paginationParams, Certifications_MainParam searchParam, List<string> roleList);
        Task<OperationResult> GetSubDetail(Certifications_SubParam param);
        Task<OperationResult> DownloadFile(Certifications_DownloadFileModel param);
        Task<OperationResult> CheckExistedData(Certifications_SubModel param);
        Task<OperationResult> PutData(Certifications_SubMemory input);
        Task<OperationResult> PostData(Certifications_SubMemory input);
        Task<OperationResult> DeleteData(Certifications_SubModel data, string userName);
        Task<OperationResult> DownloadExcel(Certifications_MainParam param, List<string> roleList);
    }
}