using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_9_DocumentManagement
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(DocumentManagement_MainParam param);
        Task<List<DocumentManagement_TypeheadKeyValue>> GetEmployeeList(DocumentManagement_SubParam param);
        Task<PaginationUtility<DocumentManagement_MainData>> GetSearchDetail(PaginationParam paginationParams, DocumentManagement_MainParam searchParam, List<string> roleList);
        Task<OperationResult> GetSubDetail(DocumentManagement_SubParam param);
        Task<OperationResult> DownloadFile(DocumentManagement_DownloadFileModel param);
        Task<OperationResult> CheckExistedData(DocumentManagement_SubModel param);
        Task<OperationResult> PutData(DocumentManagement_SubMemory input);
        Task<OperationResult> PostData(DocumentManagement_SubMemory input);
        Task<OperationResult> DeleteData(DocumentManagement_SubModel data, string userName);
        Task<OperationResult> DownloadExcel(DocumentManagement_MainParam param, List<string> roleList);
    }
}