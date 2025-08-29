using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_14_ContractTypeSetup
    {
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language);
        Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListScheduleFrequency(string language);
        Task<List<KeyValuePair<string, string>>> GetListAlertRule(string language);
        Task<OperationResult> Add(ContractTypeSetupDto data, string userName);
        Task<OperationResult> Edit(ContractTypeSetupDto data, string userName);
        Task<OperationResult> Delete(ContractTypeSetupDto data);
        Task<List<HRMSEmpContractTypeDetail>> GetDataDetail(ContractTypeSetupParam param);
        Task<PaginationUtility<ContractTypeSetupDto>> GetDataPagination(PaginationParam pagination, ContractTypeSetupParam param);
        Task<OperationResult> DownloadExcel(ContractTypeSetupParam param);
    }
}