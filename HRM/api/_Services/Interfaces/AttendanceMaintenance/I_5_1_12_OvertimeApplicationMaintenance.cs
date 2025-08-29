using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_12_OvertimeApplicationMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(OvertimeApplicationMaintenance_Param param, List<string> roleList);
        Task<OperationResult> GetOvertimeParam(OvertimeApplicationMaintenance_Param param);
        Task<OvertimeApplicationMaintenance_Main> GetShiftTime(OvertimeApplicationMaintenance_Param param);
        Task<PaginationUtility<OvertimeApplicationMaintenance_Main>> GetSearchDetail(PaginationParam paginationParams, OvertimeApplicationMaintenance_Param searchParamList);
        Task<OperationResult> IsExistedData(OvertimeApplicationMaintenance_Param param);
        Task<OperationResult> PutData(OvertimeApplicationMaintenance_Main input, string username);
        Task<OperationResult> PostData(OvertimeApplicationMaintenance_Main input, string username);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string username);
        Task<OperationResult> DeleteData(OvertimeApplicationMaintenance_Main data);
        Task<OperationResult> ExportExcel(OvertimeApplicationMaintenance_Param param);

    }
}