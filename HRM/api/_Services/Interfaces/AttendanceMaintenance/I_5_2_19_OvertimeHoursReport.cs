using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_19_OvertimeHoursReport
    {
        Task<List<OvertimeHoursReport>> GetData(OvertimeHoursReportParam param, List<string> roleList);
        Task<OperationResult> Export(OvertimeHoursReportParam param, List<string> roleList,string userName);
        Task<List<KeyValuePair<string, string>>> Query_Factory_AddList(string userName, string language);
        Task<List<KeyValuePair<string, string>>> Query_DropDown_List(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string Language, string factory);
        Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string Language);
    }
}