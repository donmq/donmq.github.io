using API.Dtos.Manage.PositionManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IPositionManageService
    {
        Task<PaginationUtility<PositionManageDto>> GetAllPosition(PaginationParam pagination, bool isPaging);
        Task<PositionManageDto> GetDetailPosition(int IDPosition);
        Task<OperationResult> AddPosition(PositionManageDto PositionAndPosLang);
        Task<OperationResult> EditPosition(PositionManageDto PositionAndPosLang);
        Task<OperationResult> RemovePosition(int IDPosition);
        Task<OperationResult> Download(PositionManageDto dto);
    }
}