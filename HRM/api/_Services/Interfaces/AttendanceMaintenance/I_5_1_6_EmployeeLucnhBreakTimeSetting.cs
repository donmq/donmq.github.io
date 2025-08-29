using API.DTOs.AttendanceMaintenance;

namespace API._Services.Interfaces.AttendanceMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_5_1_6_EmployeeLucnhBreakTimeSetting
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<PaginationUtility<HRMS_Att_LunchtimeDto>> GetDataPagination(PaginationParam pagination, EmployeeLunchBreakTimeSettingParam param, List<string> roleList);
        Task<OperationResult> DownloadExcelTemplate();
        Task<OperationResult> DownloadExcel(EmployeeLunchBreakTimeSettingParam param, List<string> roleList, string userName);
        Task<OperationResult> UploadData(IFormFile file, List<string> role_List, string userName);
        Task<OperationResult> Delete(HRMS_Att_LunchtimeDto data);
    }
}