using API.DTOs.SalaryMaintenance;
using API.Models;

namespace API._Services.Interfaces.SalaryMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_7_1_20_NightShiftSubsidyMaintenance
    {
        Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName);
        Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language);
        Task<OperationResult> CheckData(NightShiftSubsidyMaintenanceDto_Param param);
        Task<OperationResult> ExcuteQuery(NightShiftSubsidyMaintenanceDto_Param param, string account);
    }
}