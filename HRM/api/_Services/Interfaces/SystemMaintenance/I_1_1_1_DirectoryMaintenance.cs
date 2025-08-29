using API.DTOs;
using API.DTOs.SystemMaintenance;

namespace API._Services.Interfaces.SystemMaintenance
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface I_1_1_1_DirectoryMaintenance
    {
        Task<PaginationUtility<DirectoryMaintenance_Data>> Search(PaginationParam paginationParams, DirectoryMaintenance_Param param);
        Task<List<KeyValuePair<string, string>>> GetParentDirectoryCode();
        Task<OperationResult> Add(DirectoryMaintenance_Data param);
        Task<OperationResult> Update(DirectoryMaintenance_Data param);
        Task<OperationResult> Delete(string directoryCode);
    }
}