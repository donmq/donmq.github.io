

using API.DTOs.CompulsoryInsuranceManagement;

namespace API._Services.Interfaces.CompulsoryInsuranceManagement
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_6_1_4_NewEmployeesCompulsoryInsurancePremium
    {
        Task<List<KeyValuePair<string, string>>> GetDropDownList(NewEmployeesCompulsoryInsurancePremium_Param param, List<string> roleList);
        Task<OperationResult> Process(NewEmployeesCompulsoryInsurancePremium_Param param, string userName);
    }
}