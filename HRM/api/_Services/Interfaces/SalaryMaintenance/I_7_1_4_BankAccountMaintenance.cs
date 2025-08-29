using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
     [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_4_BankAccountMaintenance
    {
        Task<PaginationUtility<BankAccountMaintenanceDto>> GetDataPagination(PaginationParam pagination, BankAccountMaintenanceParam param);
        Task<OperationResult> Create(BankAccountMaintenanceDto dto);
        Task<OperationResult> Update(BankAccountMaintenanceDto dto);
        Task<OperationResult> Delete(BankAccountMaintenanceDto dto);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<OperationResult> DownloadFileExcel(BankAccountMaintenanceParam param, string userName);
        Task<OperationResult> DownloadFileTemplate();
        Task<OperationResult> UploadFileExcel(BankAccountMaintenanceUpload param, List<string> roleList, string userName);
    }
}