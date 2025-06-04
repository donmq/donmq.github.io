using API.Dtos.Common;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAreaManagementService
    {
        Task<List<AreaInformation>> GetAll();
        Task<bool> Add(AreaInformation areaInformation);
        Task<bool> Edit(AreaInformation areaInformation);
    }
}