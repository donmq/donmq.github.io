using API.Dtos.SeaHr;

namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IPermissionRightsService
    {
        Task<List<PermissionRightsDTO>> GetData(PermissionParam param);
        Task<PaginationUtility<PermissionRightsDTO>> GetDataPagination( PaginationParam pagination, PermissionParam param);
        Task<List<KeyValuePair<int, string>>> GetListParts();
        Task<OperationResult> ExportExcel(PermissionParam param);
    }
}