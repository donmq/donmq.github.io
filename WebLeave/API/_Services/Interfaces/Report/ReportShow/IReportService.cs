using API.Dtos.Report.ReportShow;
using API.Helpers.Params.ReportShow;
namespace API._Services.Interfaces.Report
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IReportService
    {
        Task<ReportIndexViewModelDTO> ReportShow(ReportShowParam param);
        Task<List<ReportShowModelDTO>> ReportGridDetail(ReportGridDetailParam param);
        Task<List<ReportShowModelDTO>> ReportDateDetail(ReportDateDetailParam param);
        Task<OperationResult> ExportDateDetail(ExportExcelDateDto model);
        Task<OperationResult> ExportGridDetail(ExportExcelGridDto model);

    }
}