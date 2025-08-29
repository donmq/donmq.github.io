using API.DTOs.SalaryMaintenance;
using API.Helper.Params.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_17_MonthlySalaryMasterFileBackupQuery
    {
        Task<OperationResult> GetDataPagination(PaginationParam pagination, MonthlySalaryMasterFileBackupQueryParam param);
        Task<PaginationUtility<SalaryDetailDto>> GetSalaryDetails(PaginationParam pagination, string probation, string factory, string employeeID, string language, string yearMonth);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListPositionTitle(string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language);
        Task<List<KeyValuePair<string, string>>> GetListTechnicalType(string language);
        Task<List<KeyValuePair<string, string>>> GetListExpertiseCategory(string language);
        Task<OperationResult> DownloadFileExcel(MonthlySalaryMasterFileBackupQueryParam param, string userName);
        Task<OperationResult> Execute(MonthlySalaryMasterFileBackupQueryParam param, string userName);
    }
}