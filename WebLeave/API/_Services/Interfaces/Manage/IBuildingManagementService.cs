using API.Dtos.Common;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IBuildingManagementService
    {
        Task<List<BuildingInformation>> GetAll();
        Task<bool> Add(BuildingInformation buildingInformation);
        Task<bool> Edit(BuildingInformation buildingInformation);
        Task<List<KeyValuePair<int, string>>> GetListArea();
    }
}