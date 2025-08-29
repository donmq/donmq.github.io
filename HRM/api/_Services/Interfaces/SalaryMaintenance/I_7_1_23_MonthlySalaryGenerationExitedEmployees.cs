using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_23_MonthlySalaryGenerationExitedEmployees
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(MonthlySalaryGenerationExitedEmployees_Param param, List<string> roleList);
        Task<OperationResult> CheckCloseStatus(MonthlySalaryGenerationExitedEmployees_Param param);
        Task<OperationResult> Execute(MonthlySalaryGenerationExitedEmployees_Param param, string userName);
    }
}