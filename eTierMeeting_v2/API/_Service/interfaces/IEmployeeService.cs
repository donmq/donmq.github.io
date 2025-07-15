using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IEmployeeService
    {
        Task<object> GetEmployeeScanByNumBer(string employeeNumber);
        Task<PageListUtility<EmployAdminDto>> SearchEmploy(PaginationParams paginationParams, string keyword);
        Task<OperationResult> UpdateEmploy(EmployeeDto employee, string userName, string lang);
        Task<OperationResult> AddEmploy(EmployeeDto employee, string userName, string lang);
        Task<OperationResult> RemoveEmploy(string empNumber, string lang);
        Task<List<EmployExportDto>> ExportExcelEmploy();
        void PutStaticValue(ref Worksheet ws);
    }
}