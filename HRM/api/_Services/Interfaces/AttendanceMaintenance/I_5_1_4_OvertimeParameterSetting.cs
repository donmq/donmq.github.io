using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_4_OvertimeParameterSetting
    {
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string language);
        Task<OperationResult> Create(HRMS_Att_Overtime_ParameterDTO data);
        Task<OperationResult> Update(HRMS_Att_Overtime_ParameterDTO data);
        Task<OperationResult> DownloadFileExcel(HRMS_Att_Overtime_ParameterParam param, string userName);
        Task<OperationResult> DownloadFileTemplate();
        Task<OperationResult> UploadFileExcel(HRMS_Att_Overtime_ParameterUploadParam param, string userName);
        Task<PaginationUtility<HRMS_Att_Overtime_ParameterDTO>> GetDataPagination(PaginationParam pagination, HRMS_Att_Overtime_ParameterParam param);

    }
}