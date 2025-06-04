using API.Dtos.SeaHr;
using API.Dtos.SeaHr.ViewConfirmDaily;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IViewConfirmDailyService
    {
        // Task<List<ViewDataModel>> GetDataViewData(IEnumerable<ViewConfirmDailyDTO> model, List<DateTime> allDates, DateTime d1, DateTime d2);
        Task<PaginationUtility<ViewConfirmDailyDTO>> GetViewConfirmDaily(string lang, string startTime, string endTime, PaginationParam pagination, bool isPaging = true);
        Task<OperationResult> ExportToData(ViewConfirmDailyParam param, PaginationParam pagination);
    }
}