using API.Dtos.Leave.Personal;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeavePersonalService
    {
        Task<PersonalDataViewModel> GetData(string userId);
        Task<PersonalDataViewModel> GetDataDetail(string empNumber);
        Task<bool> DeleteLeaveDataPerson(int leaveId, int empId);
        Task<OperationResult> AddLeaveDataPersonal(LeavePersonalDto leavePersonalDto, string userId);
    }
}