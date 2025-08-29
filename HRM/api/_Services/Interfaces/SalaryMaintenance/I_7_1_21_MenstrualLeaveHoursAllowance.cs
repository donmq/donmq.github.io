using API.DTOs.SalaryMaintenance;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_21_MenstrualLeaveHoursAllowance
    {
        Task<OperationResult> CheckData(MenstrualLeaveHoursAllowanceParam param);
        Task<OperationResult> Execute(MenstrualLeaveHoursAllowanceParam param);

        Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
    }
}