using API.Dtos.Manage.CategoryManagement;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ICategoryService
    {
        Task<PaginationUtility<CategoryDto>> GetAll(PaginationParam param);
        Task<OperationResult> Create(CategoryDetailDto category);
        Task<CategoryDetailDto> GetEditDetail(int id);
        Task<OperationResult> Update(CategoryDetailDto category);
    }
}