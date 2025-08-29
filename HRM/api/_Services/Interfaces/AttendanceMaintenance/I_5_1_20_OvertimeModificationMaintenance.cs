using API.DTOs;
using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_20_OvertimeModificationMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<OperationResult> GetWorkShiftType(OvertimeModificationMaintenanceParam param);
        Task<List<KeyValuePair<string, string>>> GetListHoliday(string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<PaginationUtility<OvertimeModificationMaintenanceDto>> GetData(PaginationParam pagination, OvertimeModificationMaintenanceParam param);
        Task<ClockInClockOut> GetWorkShiftTypeTime(string work_Shift_Type, string date, string factory);
        Task<ClockInClockOut> GetClockInTimeAndClockOutTimeByEmpIdAndDate(string employee_ID, string date);
        Task<OperationResult> Create(OvertimeModificationMaintenanceDto model, string userName);
        Task<OperationResult> Edit(OvertimeModificationMaintenanceDto model, string userName);
        Task<OperationResult> Delete(OvertimeModificationMaintenanceDto model, string userName);
    }
}