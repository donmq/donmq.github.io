using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_22_MonthlySalaryGeneration
    {
        Task<OperationResult> CheckData(MonthlySalaryGenerationParam param);
        Task<OperationResult> MonthlySalaryGenerationExecute(MonthlySalaryGenerationParam param);
        Task<OperationResult> MonthlyDataLockExecute(MonthlyDataLockParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
    }
}