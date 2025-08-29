using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactoryAdd(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<PaginationUtility<ResignedEmployeeMain>> GetDataPagination(PaginationParam pagination, ResignedEmployeeParam param, bool isPaging = true);
        Task<ResignedEmployeeDetail> Query(ResignedEmployeeParam param);
        Task<OperationResult> GetEmpInfo(ResignedEmployeeParam param);
        Task<(List<LeaveDetailDisplay> Leaves, List<LeaveDetailDisplay> Allowances)> GetResignedDetail(ResignedEmployeeDetailParam param);
        Task<OperationResult> ExportExcel(PaginationParam pagination, ResignedEmployeeParam param);
        Task<OperationResult> Add(ResignedEmployeeDetail data, string userName);
        Task<OperationResult> Edit(ResignedEmployeeDetail data, string userName);
        Task<List<KeyValuePair<string, string>>> GetEmployeeIDByFactorys(string factory);
        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
    }
}