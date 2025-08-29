using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_15_ChildcareSubsidyGeneration
    {
        Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroupByFactory(string factory, string language);
        Task<OperationResult> CheckParam(ChildcareSubsidyGenerationParam param);
        Task<int> GetTotaTab2(ChildcareSubsidyGenerationParam param);       
        Task<OperationResult> ExcuteTab1(ChildcareSubsidyGenerationParam param, string userName);
        Task<OperationResult> ExcuteTab2(ChildcareSubsidyGenerationParam param, string userName);
    }
}