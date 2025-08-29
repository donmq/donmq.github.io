using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_11_UnpaidLeave
    {
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListLeaveReason(string language);
        Task<List<string>> GetEmployeeID();
        Task<List<HRMS_Emp_Unpaid_LeaveDto>> GetEmployeeData(string factory, string employeeID, string language);
        Task<OperationResult> GetSeq(string division, string factory, string employeeID);
        Task<PaginationUtility<HRMS_Emp_Unpaid_LeaveDto>> GetDataPagination(PaginationParam pagination, UnpaidLeaveParam param, List<string> roleList);
        Task<OperationResult> DownloadExcel(UnpaidLeaveParam param, List<string> roleList, string userName);
        Task<OperationResult> AddNew(AddAndEditParam param, string userName);
        Task<OperationResult> Edit(AddAndEditParam param, string userName);
        Task<OperationResult> Delete(HRMS_Emp_Unpaid_LeaveDto data);
    }
}