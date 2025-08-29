
using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_2_2_EmergencyContactsSheetReport
    {
        Task<int> GetTotalRows(EmergencyContactsReportParam param, List<string> roleList);
        Task<OperationResult> DownloadExcel(EmergencyContactsReportParam param, List<string> roleLis, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string lang);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, List<string> roleList, string lang);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang);
    }
}