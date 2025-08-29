
using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_7_FactoryComparison
    {
        Task<PaginationUtility<HRMS_Basic_Factory_ComparisonDto>> GetData(PaginationParam pagination, string kind);
        Task<List<KeyValuePair<string, string>>> GetFactories();
        Task<List<KeyValuePair<string, string>>> GetDivisions();
        Task<OperationResult> Create(List<HRMS_Basic_Factory_ComparisonDto> models, string currentUserUpdate);
        Task<OperationResult> Delete(HRMS_Basic_Factory_ComparisonDto model);
    }
}