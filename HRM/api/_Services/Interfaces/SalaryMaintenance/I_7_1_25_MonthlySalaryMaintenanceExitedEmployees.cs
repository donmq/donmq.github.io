using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_25_MonthlySalaryMaintenanceExitedEmployees
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<PaginationUtility<D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain>> GetDataPagination(PaginationParam pagination, D_7_25_MonthlySalaryMaintenanceExitedEmployeesSearchParam param);
        Task<D_7_25_Query_Sal_Monthly_Detail_Result_Source> Get_MonthlyAttendanceData_MonthlySalaryDetail(D_7_25_GetMonthlyAttendanceDataDetailParam param);
        Task<OperationResult> Update(D_7_25_MonthlySalaryMaintenance_Update data);
        Task<OperationResult> Delete(D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain data);
    }
}