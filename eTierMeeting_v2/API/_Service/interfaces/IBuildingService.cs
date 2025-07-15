using Machine_API.DTO;
using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IBuildingService
    {
        Task<List<BuildingDto>> GetAllBuilding();
        Task<List<BuildingDto>> GetBuildingByPdcID(int id);
        Task<List<BuildingDto>> GetBuildingByCellCodeAndPDC(string cellCode, int? idPDC);
        Task<List<BuildingDto>> GetBuildingByCellCode(string cellCode);
        Task<List<BuildingDto>> GetAllBuildingByID(int idPDC);
        Task<PageListUtility<BuildingDto>> GetListBuilding(PaginationParams paginationParams);
        Task<PageListUtility<BuildingDto>> SearchBuilding(PaginationParams paginationParams, string keyword);
        Task<OperationResult> AddBuilding(BuildingDto model_Dto, string lang);
        Task<OperationResult> UpdateBuilding(BuildingDto model_Dto, string lang);
        Task<OperationResult> RemoveBuilding(BuildingDto model_Dto, string lang);
    }
}