

using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListLevel(string lang);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string lang);
        Task<OperationResult> DownloadExcel(MonthlyEmployeeStatusChangesSheet_ByGender_Param param, string userName);
        Task<int> GetTotalRows(MonthlyEmployeeStatusChangesSheet_ByGender_Param param);
    }
}