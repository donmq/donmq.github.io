
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IAssetsLendMaintainService
    {
        Task<PageListUtility<AssetsLendMaintainDto>> GetDataPagination(PaginationParams pagination, AssetsLendMaintainParam param);
        Task<byte[]> DownloadExcel(AssetsLendMaintainParam param);
        Task<OperationResult> UploadExcel(IFormFile file);
        Task<OperationResult> Update(AssetsLendMaintainDto data);
        Task<List<KeyValuePair<string, string>>> GetListLendTo();
    }
}