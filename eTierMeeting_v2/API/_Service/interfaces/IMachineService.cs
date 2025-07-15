
using Machine_API.Helpers.Attributes;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IMachineService
    {
        Task<object> GetMachineByID(string idMachine, string lang);

        Task<object> MoveMachine(string fromEmploy, string idMachine, string toEmploy, string fromPlno, string toPlno, string user);

        string GetMachineName(string idMachine, string lang);
    }
}