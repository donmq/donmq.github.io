using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_27_LoanedMonthlyAttendanceDataMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<OperationResult> GetEmployeeData(string factory, string att_Month, string employeeID, string language);
        Task<List<string>> GetEmployeeID(string factory);
        Task<PaginationUtility<LoanedMonthlyAttendanceDataMaintenanceDto>> GetDataPagination(PaginationParam pagination, LoanedMonthlyAttendanceDataMaintenanceParam param);
        Task<Detail> GetDataDetail(LoanedMonthlyAttendanceDataMaintenanceDto param);
        Task<OperationResult> DownloadExcel(LoanedMonthlyAttendanceDataMaintenanceParam param, string userName);
        Task<OperationResult> AddNew(LoanedMonthlyAttendanceDataMaintenanceDto data, string userName);
        Task<OperationResult> Edit(LoanedMonthlyAttendanceDataMaintenanceDto data, string userName);
    }
}