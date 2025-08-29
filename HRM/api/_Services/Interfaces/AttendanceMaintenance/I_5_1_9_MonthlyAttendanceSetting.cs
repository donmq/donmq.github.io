using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_9_MonthlyAttendanceSetting
    {
        Task<PaginationUtility<HRMS_Att_Use_Monthly_LeaveDto>> GetDataPagination(PaginationParam pagination, MonthlyAttendanceSettingParam param, string userName);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetLeaveTypes(string language);
        Task<List<KeyValuePair<string, string>>> GetAllowances(string language);
        Task<OperationResult> Create(List<HRMS_Att_Use_Monthly_LeaveDto> models, string userName);
        Task<OperationResult> Edit(List<HRMS_Att_Use_Monthly_LeaveDto> models, string userName);
        Task<OperationResult> Delete(string factory, string effective_Month);
        Task<List<HRMS_Att_Use_Monthly_LeaveDto>> GetCloneData(string factory, string leave_Type, string effective_Month, string userName);
        Task<List<HRMS_Att_Use_Monthly_LeaveDto>> GetRecentData(string factory, string effective_Month, string leave_Type, string action);

        Task<OperationResult> CheckDuplicateEffectiveMonth(string factory, string effective_Month);
        
    }
}