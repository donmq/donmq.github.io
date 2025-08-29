using API.DTOs.EmployeeMaintenance;

namespace API._Services.Interfaces.EmployeeMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_4_1_18_RehireEvaluationForFormerEmployees
    {

        Task<PaginationUtility<RehireEvaluationForFormerEmployeesDto>> GetDataPagination(PaginationParam pagination, RehireEvaluationForFormerEmployeesParam param);
        Task<OperationResult> Create(RehireEvaluationForFormerEmployeesEvaluation dto);
        Task<OperationResult> Update(RehireEvaluationForFormerEmployeesEvaluation dto);
        Task<RehireEvaluationForFormerEmployeesPersonal> GetDetail(string nationality, string identification_Number);
        Task<List<string>> GetListTypeHeadIdentificationNumber(string nationality);
        Task<List<KeyValuePair<string, string>>> GetListResignationType(string language);
        Task<List<KeyValuePair<string, string>>> GetListReasonforResignation(string language);
        Task<List<KeyValuePair<string, string>>> GetListDivision(string language);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string division);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory, string division);
    }
}