using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance;

[DependencyInjection(ServiceLifetime.Scoped)]
public interface I_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance
{
    Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
    Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language, string userName);
    Task<PaginationUtility<HRMS_Att_Work_Type_DaysDto>> GetDataPagination(PaginationParam pagination, SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param);
    Task<OperationResult> AddNew(HRMS_Att_Work_Type_DaysDto param);
    Task<OperationResult> Edit(HRMS_Att_Work_Type_DaysDto param);
    Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language);
    Task<OperationResult> DownloadExcel(SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param, string userName);
}