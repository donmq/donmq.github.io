using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_5_1_18_AttendanceChangeRecordMaintenance
{
    Task<PaginationUtility<HRMS_Att_Change_RecordDto>> GetDataPagination(PaginationParam pagination, HRMS_Att_Change_Record_Params param);
    Task<OperationResult> DeleteAsync(HRMS_Att_Change_RecordDto param);
    Task<OperationResult> AddAsync(HRMS_Att_Change_RecordDto param, string userAccount, string lang);
    Task<OperationResult> UpdateAsync(HRMS_Att_Change_RecordDto param, string lang);
    Task<OperationResult> CheckExistedData(string Factory, string Att_Date, string Employee_ID);
    Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
}
