using API.Dtos.Manage.EmployeeManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IEmployeeService
    {
        Task<PaginationUtility<ListEmployeeDto>> Search(PaginationParam paginationParams, string keyword, string lang);
        Task<OperationResult> UpdateEmploy(EmployeeDto employee);
        Task<OperationResult> UpdateInDetail(EmployExportDto employee);
        Task<OperationResult> ExportExcelEmploy();
        Task<EmployExportDto> getDataDetail(int EmpID, string lang);
        Task<List<KeyValuePair<string, string>>> ListGroupBase();
        Task<List<KeyValuePair<string, string>>> ListPositionID();
        Task<List<KeyValuePair<string, string>>> ListPartID(int DeptID);
        Task<List<KeyValuePair<string, string>>> ListDeptID();
        Task<PaginationUtility<LeaveDataDto>> SearchDetail(PaginationParam param, int EmployeeId , int CategoryId , int Year, string lang);
        Task<List<KeyValuePair<string, string>>> ListCataLog(string lang);
        Task<OperationResult> RemoveEmploy(int empID);
        Task<OperationResult>changeVisible(int empID);

    }
}