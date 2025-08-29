using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_24_MonthlySalaryMaintenance
    {
        Task<PaginationUtility<MonthlySalaryMaintenanceDto>> GetDataPagination(PaginationParam pagination, MonthlySalaryMaintenanceParam param);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<List<string>> GetListTypeHeadEmployeeID(string factory);
        Task<MonthlySalaryMaintenance_Personal> GetDetailPersonal(string factory, string employee_ID);
        Task<MonthlySalaryMaintenanceDetail> GetDetail(MonthlySalaryMaintenanceDto data);
        Task<OperationResult> Update(MonthlySalaryMaintenance_Update dto);
        Task<OperationResult> Delete(MonthlySalaryMaintenance_Delete dto);
    }
}