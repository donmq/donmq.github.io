
using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_15_ContractManagement
    {
        Task<PaginationUtility<ContractManagementDto>> GetData(PaginationParam pagination, ContractManagementParam param, List<string> roleList);
        Task<OperationResult> Create(ContractManagementDto data);
        Task<OperationResult> Update(ContractManagementDto data);
        Task<OperationResult> Delete(ContractManagementDto data);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string lang);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang);
        Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string lang);
        Task<List<KeyValuePair<string, string>>> GetListAssessmentResult(string lang);
        Task<Personal> GetPerson(PersonalParam paramPersonal);
        Task<ProbationParam> GetProbationDate(string division, string factory, string contractType);
        Task<List<KeyValuePair<string, string>>> GetEmployeeID();

    }
}