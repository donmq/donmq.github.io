using API.DTOs.RewardAndPenaltyReport;

namespace API._Services.Interfaces.RewardAndPenaltyMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_8_2_1_EmployeeRewardAndPenaltyReport
    {
        Task<OperationResult> GetTotalRows(EmployeeRewardAndPenaltyReportParam param);
        Task<OperationResult> Download(EmployeeRewardAndPenaltyReportParam param);
        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<List<KeyValuePair<string, string>>> GetListRewardPenaltyType(string language);
    }
}