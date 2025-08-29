
using API.DTOs.CompulsoryInsuranceManagement;

namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{
     [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_1_1_CompulsoryInsuranceDataMaintenance
    {
        Task<PaginationUtility<CompulsoryInsuranceDataMaintenanceDto>> GetDataPagination(PaginationParam pagination, CompulsoryInsuranceDataMaintenanceParam param);
        Task<OperationResult> Create(CompulsoryInsuranceDataMaintenanceDto dto);
        Task<OperationResult> Update(CompulsoryInsuranceDataMaintenanceDto dto);
        Task<OperationResult> Delete(CompulsoryInsuranceDataMaintenanceDto dto);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language);
        Task<OperationResult> DownloadFileExcel(CompulsoryInsuranceDataMaintenanceParam param, string userName);
        Task<OperationResult> DownloadFileTemplate();
        Task<OperationResult> UploadFileExcel(CompulsoryInsuranceDataMaintenance_Upload param, List<string> roleList, string userName);
    }
}