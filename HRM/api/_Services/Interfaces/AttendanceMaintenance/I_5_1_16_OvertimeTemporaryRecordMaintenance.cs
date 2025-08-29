
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_16_OvertimeTemporaryRecordMaintenance
    {
        Task<PaginationUtility<HRMS_Att_Overtime_TempDto>> GetData(PaginationParam pagination, HRMS_Att_Overtime_TempParam param);
        Task<OperationResult> Create(HRMS_Att_Overtime_TempDto data, string userName);
        Task<OperationResult> Update(HRMS_Att_Overtime_TempDto data, string userName);
        Task<OperationResult> Delete(HRMS_Att_Overtime_TempDto data);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string lang, List<string> roleList);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string lang);
        Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string lang);
        Task<List<KeyValuePair<string, string>>> GetListHoliday(string lang);
        Task<ClockInOutTempRecord> GetClockInOutByTempRecord(OvertimeTempPersonalParam param);
        Task<KeyValuePair<string, string>> GetShiftTimeByWorkShift(string factory, string workShiftType, string date);
        Task<OperationResult> DownloadFileExcel(HRMS_Att_Overtime_TempParam param, string userName);

    }
}