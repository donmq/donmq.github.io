using API.DTOs.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_5_PayslipDeliveryByEmailMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<OperationResult> CheckDuplicate(string factory, string employeeID);
        Task<PaginationUtility<D_7_5_PayslipDeliveryByEmailMaintenanceDto>> GetDataPagination(PaginationParam pagination, PayslipDeliveryByEmailMaintenanceParam param);
        Task<OperationResult> DownloadExcel(PayslipDeliveryByEmailMaintenanceParam param, string userName, bool isTemplate);
        Task<OperationResult> UploadData(IFormFile file, string userName);
        Task<OperationResult> AddNew(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName);
        Task<OperationResult> Edit(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName);
        Task<OperationResult> Delete(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName);
    }
}