using API.Dtos.Report.ReportChart;
namespace API._Services.Interfaces.Report
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IReportChartService
    {
        Task<ReportChartDto> GetDataChart();
        Task<ReportChartDto> GetDataChartInArea(int areaID);
    }
}