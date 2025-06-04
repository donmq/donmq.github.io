using API.Dtos.Leave;
using API.Dtos.Leave.Personal;
using API.Dtos.Leave.Representative;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveRepresentativeService
    {
        Task<bool> AddLeaveData(LeavePersonalDto leavePersonal, string userId);
        Task<List<RepresentativeDataViewModel>> GetDataLeave(string userId);
        Task<OperationResult> GetEmployeeInfo(string userId, string empNumber);
        Task<bool> DeleteLeave(List<RepresentativeDataViewModel> leaveDatas);
        Task<List<LeaveDataViewModel>> GetListOnTime(string userId, int leaveId);
    }
}