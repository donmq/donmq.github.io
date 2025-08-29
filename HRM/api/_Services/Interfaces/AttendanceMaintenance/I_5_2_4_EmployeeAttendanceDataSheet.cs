using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_2_4_EmployeeAttendanceDataSheet
    {
        Task<int> GetCountRecords(EmployeeAttendanceDataSheetParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language);
        Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<OperationResult> DownloadFileExcel(EmployeeAttendanceDataSheetParam param,  string userName);

    }
}