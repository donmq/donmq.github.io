using Machine_API.DTO;
using Machine_API.Helpers.Attributes;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategory();
    }
}