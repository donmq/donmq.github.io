using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHistoryInventoryService
    {
        Task<PageListUtility<HistoryInventoryDto>> SearchHistoryInventory(HistoryInventoryParams historyInventoryParams, PaginationParams paginationParams);
        Task<List<DataHistoryInventoryDto>> GetDatalHistoryInventoryById(int historyInventoryID);
        bool CheckDateHistoryInventory(string checkDate);
        void PutStaticValue(ref Worksheet ws, HistoryInventoryDto dataHistory);
        void CustomStyle(ref Cell cellCustom);
        void PutStaticValue1(ref Worksheet ws, HistoryInventoryExportPdfDto history);
        void CustomStyle1(ref Cell cellCustom);
        Task<List<HistoryInventoryLineDto>> GetListPlnoHistotry(string dateSearch, int idBuilding, int? isCheck);
        Task<ResultAllInventoryDto> GetListDetailHistoryPlno(string plnoId, string timeSoKiem, string timePhucKiem, string timeRutKiem, string lang);
        Task<ResultAllInventoryDto> GetAllDetailHistoryPlno(ReportKiemKeParam param);
        Task<List<HistoryInventoryExportPdfDto>> GetDataPdfByDay(string date);

    }
}