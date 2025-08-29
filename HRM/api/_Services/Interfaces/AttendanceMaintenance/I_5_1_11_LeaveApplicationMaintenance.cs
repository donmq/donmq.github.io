using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_11_LeaveApplicationMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(LeaveApplicationMaintenance_Param param, List<string> roleList);
        Task<PaginationUtility<LeaveApplicationMaintenance_Main>> GetSearchDetail(PaginationParam paginationParams, LeaveApplicationMaintenance_Param searchParam);
        Task<OperationResult> IsExistedData(LeaveApplicationMaintenance_Param param);
        Task<OperationResult> PutData(LeaveApplicationMaintenance_Main input, string username);
        Task<OperationResult> PostData(LeaveApplicationMaintenance_Main input, string username);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string username);
        Task<OperationResult> DeleteData(LeaveApplicationMaintenance_Main data);
        Task<OperationResult> ExportExcel(LeaveApplicationMaintenance_Param param);

    }
}