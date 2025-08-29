
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListLevel(string lang);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string lang);
        Task<OperationResult> DownloadExcel(MonthlyEmployeeStatusParam param, string userName);
        Task<int> GetTotalRows(MonthlyEmployeeStatusParam param);
        int Total_Begin_Count(MonthlyEmployeeStatusParam param, TableParam paramTable);
        int Total_NewHires_Count(MonthlyEmployeeStatusParam param, TableParam paramTable);
        List<Department> Employee_Dept(MonthlyEmployeeStatusParam param, TableParam paramTable);
    }
}