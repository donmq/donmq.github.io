using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_1_FactoryCalendar
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(FactoryCalendar_MainParam param,string userName);
        Task<OperationResult> GetSearchDetail(PaginationParam paginationParams, FactoryCalendar_MainParam searchParam);
        Task<OperationResult> CheckExistedData(string Division, string Factory, string Att_Date);
        Task<OperationResult> PutData(FactoryCalendar_Table input);
        Task<OperationResult> PostData(FactoryCalendar_Table input);
        Task<OperationResult> DownloadExcel(FactoryCalendar_MainParam param);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string username);
        Task<OperationResult> DeleteData(FactoryCalendar_Table input);
    }
}