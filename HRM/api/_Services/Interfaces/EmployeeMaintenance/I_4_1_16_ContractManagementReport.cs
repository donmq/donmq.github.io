using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_16_ContractManagementReport
    {
        Task<PaginationUtility<ContractManagementReportDto>> GetDataPagination(PaginationParam pagination, ContractManagementReportParam param);
        Task<OperationResult> DownloadExcel(ContractManagementReportParam param);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string lang);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang);
        Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string lang);
        Task<List<KeyValuePair<string, string>>> GetListSalaryType(string lang);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang);
    }
}