using API.Dtos.Common;
namespace API._Services.Interfaces.Leave
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveDetailService
    {
        Task<LeaveDetailDto> GetDetail(int leaveID, int userID);
        Task<OperationResult> RequestEditLeave(int? leaveID, string ReasonAdjust);
        Task<OperationResult> EditCommentArchive(int userID, int leaveID, int commentArchiveID);
        Task<OperationResult> EditApproval(string userName, int userID, int empId, int? slEditApproval, int leaveID, string notiText);
        Task<OperationResult> SendNotitoUser(int? empid, string userName, string notitext, int? leaveid);
    }
}