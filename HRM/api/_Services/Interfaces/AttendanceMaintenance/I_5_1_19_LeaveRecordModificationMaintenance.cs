using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_5_1_19_LeaveRecordModificationMaintenance
{
    #region get data main
    // 2.19 query in specs
    Task<PaginationUtility<Leave_Record_Modification_MaintenanceDto>> GetDataPagination(PaginationParam pagination, Leave_Record_Modification_MaintenanceSearchParamDto param, List<string> roleList);
    #endregion

    Task<OperationResult> DeleteAsync(Leave_Record_Modification_MaintenanceDto param, string userAccount);
    // add page
    Task<OperationResult> AddAsync(Leave_Record_Modification_MaintenanceDto param);
    Task<OperationResult> UpdateAsync(Leave_Record_Modification_MaintenanceDto param);
    Task<OperationResult> CheckExistedData(string Factory, string Employee_ID, string Leave_Code, string Leave_Date);
    Task<List<KeyValuePair<string, string>>> GetListLeave(string language);
    Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName);
    Task<OperationResult> GetWorkShiftType(Leave_Record_Modification_MaintenanceSearchParamDto param);
    Task<OperationResult> DownloadFileExcel(Leave_Record_Modification_MaintenanceSearchParamDto param, string userName);

}
