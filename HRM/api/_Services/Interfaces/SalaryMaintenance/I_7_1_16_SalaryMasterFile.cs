using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_16_SalaryMasterFile
    {
        Task<PaginationUtility<SalaryMasterFile_Main>> GetDataPagination(PaginationParam pagination, SalaryMasterFile_Param param);
        Task<List<KeyValuePair<string, string>>> GetFactorys(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetDepartments(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetPositionTitles(string language);
        Task<List<KeyValuePair<string, string>>> GetSalaryTypes(string language);
        Task<List<KeyValuePair<string, string>>> GetTechnicalTypes(string language);
        Task<List<KeyValuePair<string, string>>> GetExpertiseCategorys(string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language);
        Task<OperationResult> DownloadFileExcel(SalaryMasterFile_Param param, string userName);
        Task<SalaryMasterFile_Detail> GetDataQueryPage(PaginationParam pagination, string factory, string employee_ID, string language);
    }
}