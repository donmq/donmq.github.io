using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_18_EmployeeOvertimeDataSheet 
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory (string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<OperationResult> GetPagination(EmployeeOvertimeDataSheetParam param);
        Task<OperationResult> ExportExcel(EmployeeOvertimeDataSheetParam param);
    }
}