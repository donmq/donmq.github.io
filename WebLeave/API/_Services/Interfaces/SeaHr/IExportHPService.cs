using API.Dtos.SeaHr.ExportHP;
using API.Helpers.Params;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IExportHPService
    {
        Task<List<ExportLeave>> GetData(ExportHPParam param);
        Task<PaginationUtility<ExportLeave>> PaginationData(ExportHPParam param, PaginationParam paginationParam);
        Task<OperationResult> DownloadExcel(ExportHPParam param, string typeFile);

    }
}