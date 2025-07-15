using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ICellService
    {
        Task<List<CellDto>> GetAllCell();
        Task<List<CellDto>> GetAllCellAdmin();
        Task<List<CellDto>> GetListCellByBuildingID(int buildingID);
        Task<List<CellDto>> GetListCellExistPlnoByBuildingID(int buildingID);
        Task<object> GetListCellByPdcID(int pdcID);
        Task<PageListUtility<CellDto>> GetListCell(PaginationParams paginationParams);
        Task<PageListUtility<CellDto>> SearchCell(PaginationParams paginationParams, string keyword);
        Task<OperationResult> AddCell(CellDto model_Dto, string lang);
        Task<OperationResult> UpdateCell(CellDto model_Dto, string lang);
        Task<OperationResult> RemoveCell(CellDto model_Dto, string lang);
        Task<List<CellExportDto>> ExportExcelCell();
        Task<Cells> GetDataCell(string cellCode);

    }
}