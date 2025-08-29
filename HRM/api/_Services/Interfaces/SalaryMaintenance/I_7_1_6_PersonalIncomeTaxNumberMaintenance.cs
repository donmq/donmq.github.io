using API.DTOs.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_6_PersonalIncomeTaxNumberMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<OperationResult> CheckDuplicate(string factory, string employeeID, string year);
        Task<PaginationUtility<D_7_6_PersonalIncomeTaxNumberMaintenanceDto>> GetDataPagination(PaginationParam pagination, PersonalIncomeTaxNumberMaintenanceParam param);
        Task<OperationResult> DownloadExcel(PersonalIncomeTaxNumberMaintenanceParam param, string userName, bool isTemplate);
        Task<OperationResult> UploadData(IFormFile file, string userName);
        Task<OperationResult> AddNew(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName);
        Task<OperationResult> Edit(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName);
        Task<OperationResult> Delete(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName);
    }
}