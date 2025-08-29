using API.DTOs.BasicMaintenance;

namespace API._Services.Interfaces.BasicMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_2_1_8_ResetPassword
    {
        Task<OperationResult> ResetPassword(ResetPasswordParam param);
    }
}