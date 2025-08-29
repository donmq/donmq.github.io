using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_3_LeaveSalaryCalculationMaintenance
    {
        Task<PaginationUtility<LeaveSalaryCalculationMaintenanceDTO>> GetDataPagination(PaginationParam pagination, LeaveSalaryCalculationMaintenanceParam param);
        Task<OperationResult> Create(LeaveSalaryCalculationMaintenanceDTO data);
        Task<OperationResult> Update(LeaveSalaryCalculationMaintenanceDTO data);
        Task<OperationResult> Delete(LeaveSalaryCalculationMaintenanceDTO data);
        Task<List<KeyValuePair<string, string>>> GetListLeaveCode(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
    }
}