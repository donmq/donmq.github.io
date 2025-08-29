using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_15_AttendanceAbnormalityDataMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string language);
        Task<List<KeyValuePair<string, string>>> GetListAttendance(string language);
        Task<List<KeyValuePair<string, string>>> GetListUpdateReason(string language);
        Task<List<KeyValuePair<string, string>>> GetListHoliday(string language);
        Task<PaginationUtility<HRMS_Att_Temp_RecordDto>> GetDataPagination(PaginationParam pagination, AttendanceAbnormalityDataMaintenanceParam param);
        Task<OperationResult> AddNew(HRMS_Att_Temp_RecordDto data, string userName);
        Task<OperationResult> Edit(HRMS_Att_Temp_RecordDto data, string userName);
        Task<OperationResult> Delete(HRMS_Att_Temp_RecordDto data, string userName);
        Task<OperationResult> DownloadFileExcel(AttendanceAbnormalityDataMaintenanceParam param, string userName);

    }
}