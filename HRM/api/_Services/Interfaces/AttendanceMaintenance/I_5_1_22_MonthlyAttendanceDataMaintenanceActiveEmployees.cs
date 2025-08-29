using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees
    {
        Task<PaginationUtility<MaintenanceActiveEmployeesMain>> GetPagination(PaginationParam pagination, MaintenanceActiveEmployeesParam param);
        Task<OperationResult> Add(MaintenanceActiveEmployeesDetail data, string userName);
        Task<OperationResult> Edit(MaintenanceActiveEmployeesDetail data, string userName);
        Task<MaintenanceActiveEmployeesDetail> Detail(MaintenanceActiveEmployeesDetailParam param);
        Task<OperationResult> GetEmpInfo(ActiveEmployeeParam param);
        Task<(List<LeaveAllowance>, List<LeaveAllowance>)> GetLeaveAllowance(MaintenanceActiveEmployeesDetailParam param);
        Task<OperationResult> Download(MaintenanceActiveEmployeesParam param, string userName);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string Language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string Language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string Language);
        Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language);
        Task<List<KeyValuePair<string, string>>> Query_DropDown_List(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetEmployeeIDByFactorys(string factory);
        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
    }
}