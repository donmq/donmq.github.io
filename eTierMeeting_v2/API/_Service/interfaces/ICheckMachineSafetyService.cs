using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ICheckMachineSafetyService
    {
        Task<Machine_Safe_CheckDto> GetMachine(string idMachine, string lang);
        Task<List<KeyValuePair<string, string>>> GetListQuestion(string lang);
        Task<OperationResult> SaveMachineSafetyCheck(SurveyRequest request);
    }
}