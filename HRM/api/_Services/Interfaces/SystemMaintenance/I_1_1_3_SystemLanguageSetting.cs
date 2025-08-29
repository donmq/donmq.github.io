using API.DTOs.SystemMaintenance;
using API.Models;

namespace API._Services.Interfaces.SystemMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_1_1_3_SystemLanguageSetting
    {
        Task<PaginationUtility<HRMS_SYS_Language>> GetData(PaginationParam pagination);
        Task<OperationResult> Create(SystemLanguageSetting_Data data);
        Task<OperationResult> Update(SystemLanguageSetting_Data data);
    }
}