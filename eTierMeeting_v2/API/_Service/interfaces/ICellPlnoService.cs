using Aspose.Cells;
using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ICellPlnoService
    {
        Task<List<Hp_a15Dto>> GetAllCellPlno();
        Task<List<Hp_a15Dto>> GetListPlnoByBuildingAndCellID(int buildingID, string cellCode);
        Task<List<Hp_a15Dto>> GetListPlanoByCellIDV2(string cellCode);
        Task<List<Hp_a15Dto>> GetListPlnoByCellID(int cellID);
        Task<List<Hp_a15Dto>> GetListPlnoByPDCID(int pdcID);
        Task<List<Hp_a15Dto>> GetListPlnoByBuildingID(int buildingID);
        Task<List<Hp_a15Dto>> GetListPlnoByMultipleBuildingID(string[] listBuildingID);
        Task<List<Hp_a15Dto>> GetListPlnoByMultipleCellID(string[] listCellID);
        Task<List<Hp_a15Dto>> GetListPlnoByMultipleID(FilterListDTO listAll);
        Task<List<InventoryLineDto>> GetListPlnoToInventory(List<Hp_a15Dto> listPlano);
        Task<List<InventoryLineDto>> GetListPlnoByBuildingToInventory(string id, string checkGetData = "building");
        Task<Hp_a15Dto> GetPlace(string plno);
        Task<PageListUtility<Cell_PlnoDto>> GetListCellPlno(PaginationParams pagination);
        Task<PageListUtility<Cell_PlnoDto>> searchCellPlno(PaginationParams pagination, string keyword);
        Task<OperationResult> AddCellPlno(Cell_PlnoDto model_Dto, string lang);
        Task<OperationResult> UpdateCellPlno(Cell_PlnoDto model_Dto, string lang);
        Task<OperationResult> RemoveCellPlno(Cell_PlnoDto model_Dto, string lang);
        Task<List<CellPlnoExportDto>> ExportExcelCellPlno();
        Task<bool> IsExists(Cell_PlnoDto model_Dto);
        void PutStaticValue(ref Worksheet ws);
    }
}